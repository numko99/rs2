import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/helpers/date_time_helper.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/employee_arrangement.dart';
import 'package:iter_mobile/models/filters.dart';
import 'package:iter_mobile/providers/employee_arrangment_provider.dart';
import 'package:iter_mobile/widgets/arrangement_card.dart';
import 'package:iter_mobile/widgets/filter_chip.dart';
import 'package:iter_mobile/widgets/logo.dart';
import 'package:provider/provider.dart';

class EmployeeHomePage extends StatefulWidget {
  const EmployeeHomePage({super.key});

  @override
  _EmployeeHomePageState createState() => _EmployeeHomePageState();
}

class _EmployeeHomePageState extends State<EmployeeHomePage> {
  List<EmployeeArrangment> arrangements = [];

  final TextEditingController searchController = TextEditingController();

  int currentPage = 1;
  static const int pageSize = 10;

  Filter filter = Filter();
  String inputValue = "";

  bool displayLoader = false;
  bool hasMore = true;

  EmployeeArrangmentProvider? _employeeArrangmentProvider;
  final ScrollController _scrollController = ScrollController();
  final GlobalKey<ScaffoldState> _scaffoldKey = GlobalKey<ScaffoldState>();

  @override
  void initState() {
    super.initState();
    _employeeArrangmentProvider = context.read<EmployeeArrangmentProvider>();
    loadData(true);
    _scrollController.addListener(_loadMore);
  }

  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
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
      var searchArrangements = await _employeeArrangmentProvider?.get({
        "currentPage": loadPage,
        "pageSize": pageSize
      });

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
          backgroundColor: Colors.white,
          title: Logo(fontSize: 30, color: Colors.amber),
          leading: null,
        ),
        body: Column(
          children: [
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Wrap(
                spacing: 8.0,
                runSpacing: 4.0,
                alignment: WrapAlignment.start,
                children: [
                  Align(
                    alignment: Alignment.centerLeft,
                    child: Text("Nadolazeća putovanja", style: TextStyle(fontSize: 18, color: Colors.grey[600]))
                  ),
                  ChipFilter(
                      onFiltersChanged: () => loadData(true), filter: filter),
                ],
              ),
            ),
            Expanded(
              child: ListView.builder(
                controller: _scrollController,
                itemCount: arrangements.length,
                itemBuilder: (context, index) {
                  return Stack(
                    children: [
                      ArrangementCard(
                        id: arrangements[index].arrangement!.id,
                        image: arrangements[index].arrangement!.mainImage.image,
                        name: arrangements[index].arrangement!.name,
                        agencyName: arrangements[index].arrangement!.agencyName,
                        onReturn: () => loadData(true),
                        departureDate: DateFormat('dd.MM.yyyy')
                            .format(arrangements[index].arrangement!.startDate)),
                      if (DateTimeCustomHelper.isBeforeDateOnly(
                              arrangements[index].arrangement!.startDate, DateTime.now())
                          || DateTimeCustomHelper.areDatesEqual(
                              arrangements[index].arrangement!.startDate,
                              DateTime.now()))
                      Positioned(
                        top: 20,
                        right: 20,
                        child: Chip(backgroundColor: Colors.green, label: Text("Putovanje u toku", style: TextStyle(color: Colors.white),))),
                    ],
                  );
                },
              ),
            ),
          ],
        ));
  }
}
