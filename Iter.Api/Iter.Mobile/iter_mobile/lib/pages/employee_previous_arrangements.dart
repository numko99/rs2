import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/employee_arrangement.dart';
import 'package:iter_mobile/providers/employee_arrangment_provider.dart';
import 'package:iter_mobile/widgets/arrangement_card.dart';
import 'package:provider/provider.dart';

class EmployeePreviousArrangementsPage extends StatefulWidget {
  const EmployeePreviousArrangementsPage({super.key});

  @override
  State<EmployeePreviousArrangementsPage> createState() => _EmployeePreviousArrangementsPageState();
}

class _EmployeePreviousArrangementsPageState extends State<EmployeePreviousArrangementsPage>
    with SingleTickerProviderStateMixin {
  List<EmployeeArrangment> arrangements = [];
  TabController? _tabController;

  bool displayLoader = false;
  bool hasMore = true;

  EmployeeArrangmentProvider? _employeeArrangmentProvider;
  final ScrollController _scrollController = ScrollController();
  final GlobalKey<ScaffoldState> _scaffoldKey = GlobalKey<ScaffoldState>();

  int currentPage = 1;
  int pageSize = 10;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 2, vsync: this);
    _tabController!.addListener(_handleTabSelection);
    _employeeArrangmentProvider = context.read<EmployeeArrangmentProvider>();
    loadData(true);
    _scrollController.addListener(_loadMore);
  }

  @override
  void dispose() {
    _tabController?.dispose();
    super.dispose();
  }

  void _handleTabSelection() {
    if (_tabController!.indexIsChanging) {
      loadData(true);
    }
  }


  void _loadMore() {
    if (_scrollController.position.pixels ==
            _scrollController.position.maxScrollExtent &&
        hasMore) {
      loadData(false);
    }
  }

  Future<void> loadData(refresh) async {
    try {
      if (!displayLoader) {
        setState(() {
          displayLoader = true;
        });
      }

      int loadPage = refresh ? 1 : currentPage;
      var searchArrangements = await _employeeArrangmentProvider
          ?.get({
          "currentPage": loadPage,
          "pageSize": pageSize,
          "returnActiveArrangements": _tabController!.index == 0});

      if (searchArrangements != null && searchArrangements.result.isNotEmpty) {
        int totalPages = (searchArrangements.count ~/ pageSize) +
            (searchArrangements.count % pageSize == 0 ? 0 : 1);

        setState(() {
          if (refresh) {
            arrangements = searchArrangements.result;
          } else {
            arrangements.addAll(searchArrangements.result);
          }
          currentPage = loadPage + 1;
          hasMore = currentPage <= totalPages;
        });
      } else {
        setState(() {
          if (refresh) {
            arrangements.clear();
          }
          hasMore = false;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške: ${error.toString()}",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      key: _scaffoldKey,
      appBar: AppBar(
        toolbarHeight: 0,
        bottom: TabBar(
          labelColor: Colors.white,
          controller: _tabController,
          tabs: const [
            Tab(text: 'Aktivne Rezervacije'),
            Tab(text: 'Prethodna Putovanja'),
          ],
        ),
      ),
      body: displayLoader == true ?
      const Center(child: CircularProgressIndicator())
      : TabBarView(
        controller: _tabController,
        children: [
          arrangements.isEmpty
              ? Center(child: Text('Nema aranzmana.'))
              : ListView.builder(
                itemCount: arrangements.length,
                itemBuilder: (context, index) {
                  return Stack(
                    children: [
                      ArrangementCard(
                        id: arrangements[index].arrangement!.id,
                        image: arrangements[index].arrangement!.mainImage.image,
                        name: arrangements[index].arrangement!.name,
                        agencyName: arrangements[index].arrangement!.agencyName,
                        departureDate: DateFormat('dd.MM.yyyy')
                            .format(arrangements[index].arrangement!.startDate)),
                      Positioned(
                        top: 20,
                        right: 20,
                        child: Chip(backgroundColor: Colors.green, label: Text("Putovanje u toku", style: TextStyle(color: Colors.white),))),
                    ],
                  );
                },
              ),
          arrangements.isEmpty
              ? Center(child: Text('Nema rezervacija.'))
              : ListView.builder(
                        itemCount: arrangements.length,
                        itemBuilder: (context, index) {
                          return Stack(
                            children: [
                              ArrangementCard(
                                  id: arrangements[index].arrangement!.id,
                                  image: arrangements[index]
                                      .arrangement!
                                      .mainImage
                                      .image,
                                  name: arrangements[index].arrangement!.name,
                                  agencyName: arrangements[index]
                                      .arrangement!
                                      .agencyName,
                                  departureDate: DateFormat('dd.MM.yyyy')
                                      .format(arrangements[index]
                                          .arrangement!
                                          .startDate)),
                            Positioned(
                                  top: 20,
                                  right: 20,
                                  child: Chip(
                                      backgroundColor: Colors.red,
                                      label: Text(
                                        "Završeno",
                                        style: TextStyle(color: Colors.white),
                                      ))),
                    
                            ],
                          );
                        },
                      ),
        ],
      ),
    );
  }
}
