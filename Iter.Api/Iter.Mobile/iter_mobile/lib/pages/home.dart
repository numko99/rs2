import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/enums/arrangement_status.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/arrangement_search_response.dart';
import 'package:iter_mobile/models/arrangment.dart';
import 'package:iter_mobile/models/filters.dart';
import 'package:iter_mobile/providers/arrangment_provider.dart';
import 'package:iter_mobile/widgets/arrangement_card.dart';
import 'package:iter_mobile/widgets/filter_chip.dart';
import 'package:iter_mobile/widgets/filter_drawer.dart';
import 'package:iter_mobile/widgets/icon_text_chip.dart';
import 'package:iter_mobile/widgets/logo.dart';
import 'package:provider/provider.dart';

class HomePage extends StatefulWidget {
  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  List<ArrangementSearchResponse> arrangements = [];

  final TextEditingController searchController = TextEditingController();

  int currentPage = 1;
  static const int pageSize = 10;

  Filter filter = Filter();
  String inputValue = "";

  bool displayLoader = false;
  bool hasMore = true;

  ArrangmentProvider? _arrangmentProvider;
  final ScrollController _scrollController = ScrollController();
  final GlobalKey<ScaffoldState> _scaffoldKey = GlobalKey<ScaffoldState>();

  @override
  void initState() {
    super.initState();
    _arrangmentProvider = context.read<ArrangmentProvider>();
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
      var searchArrangements = await _arrangmentProvider?.get({
        "dateFrom": filter.startDate != null
            ? DateFormat('dd.MM.yyyy').format(filter.startDate!)
            : DateFormat('dd.MM.yyyy').format(DateTime.now()),
        "dateTo": filter.endDate != null
            ? DateFormat('dd.MM.yyyy').format(filter.endDate!)
            : null,
        "name": searchController.text,
        "arrangementStatus": ArrangementStatus.availableForReservation.index,
        "rating": filter.rating,
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
        toolbarHeight: 0,
      ),
      body: displayLoader == true
          ? const Center(child: CircularProgressIndicator())
          :  Column(
        children: [
          Card(
            child: SizedBox(
              height: 60,
              child: Padding(
                padding: const EdgeInsets.fromLTRB(0, 0.0, 0, 0),
                child: TextFormField(
                  controller: searchController,
                  onChanged: (String value) {
                    if (value.length > 3 || value.length == 0) {
                      loadData(true);
                    }
                  },
                  decoration: InputDecoration(
                      contentPadding: const EdgeInsets.all(16),
                      labelText: 'Pretraga po aranžanu, destinaciji...',
                      suffixIcon: IconButton(
                          icon: const Icon(Icons.search),
                          onPressed: () => loadData(true))),
                ),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Wrap(
              spacing: 8.0,
              runSpacing: 4.0,
              alignment: WrapAlignment.start,
              children: [
                Align(
                  alignment: Alignment.centerLeft,
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      GestureDetector(
                          child: IconTextChip(
                              labelStyle: const TextStyle(
                                  fontSize: 12, color: Colors.white),
                              iconData: Icons.filter_alt_sharp,
                              label: "Filteri"),
                          onTap: () =>
                              _scaffoldKey.currentState!.openEndDrawer()),
                    ],
                  ),
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
                return ArrangementCard(
                    id: arrangements[index].id,
                    image: arrangements[index].mainImage!.image,
                    name: arrangements[index].name,
                    agencyName: arrangements[index].agencyName,
                    agencyRating: arrangements[index].agencyRating.toString(),
                    isReserved: arrangements[index].isReserved,
                    onReturn: () => loadData(true),
                    departureDate: DateFormat('dd.MM.yyyy')
                        .format(arrangements[index].startDate));
              },
            ),
          ),
        ],
      ),
      endDrawer: FilterDrawer(
          filter: filter,
          onFiltersChanged: (Filter updatedFilter) {
            setState(() {
              filter = updatedFilter;
            });
            loadData(true);
          }),
    );
  }
}
