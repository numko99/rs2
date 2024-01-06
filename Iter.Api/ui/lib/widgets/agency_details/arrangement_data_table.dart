import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';

import '../../modals/customConfirmationModal.dart';
import '../../models/arrangment.dart';
import '../../services/arrangment_provider.dart';
import '../search_button.dart';

class ArrangementDataTable extends StatefulWidget {
  const ArrangementDataTable({super.key, required this.agencyId});

  final String agencyId;

  @override
  State<ArrangementDataTable> createState() => _ArrangementDataTableState();
}

class _ArrangementDataTableState extends State<ArrangementDataTable> {
  TextEditingController searchController = TextEditingController();

  ArrangmentProvider? _arrangementProvider;
  List<Arrangement> arrangements = [];
  int? arrangementsCount;

  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 5;

  @override
  void initState() {
    super.initState();
    _arrangementProvider = context.read<ArrangmentProvider>();
    loadData();
  }

  Future<void> loadData() async {
    setState(() {
      _displayLoader = true;
    });

    var searchArrangements = await _arrangementProvider?.get({
      "currentPage": _currentPage,
      "pageSize": _pageSize,
      "agencyId": widget.agencyId,
      "name": searchController.value
    });

    setState(() {
      arrangements = searchArrangements?.result ?? [];
      arrangementsCount = searchArrangements?.count ?? 0;
      _displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: <Widget>[
            Row(
              crossAxisAlignment: CrossAxisAlignment.end,
              children: <Widget>[
                Container(
                  width: 200,
                  height: 50,
                  margin: const EdgeInsets.symmetric(horizontal: 20),
                  child: TextFormField(
                    controller: searchController,
                    decoration: InputDecoration(
                      labelText: 'Naziv',
                    ),
                    onFieldSubmitted: (value) async {
                      await loadData();
                    },
                  ),
                ),
                Container(
                  width: 150,
                  height: 35,
                  margin: const EdgeInsets.symmetric(horizontal: 10),
                  child: SearchButton(
                    onSearch: loadData,
                  ),
                ),
              ],
            ),
            Container(
              width: 150,
              height: 35,
              margin: const EdgeInsets.fromLTRB(0, 15, 0, 0),
              child: ElevatedButton(
                onPressed: () {
                  // showDialog(
                  //   context: context,
                  //   builder: (BuildContext context) {
                  //     return InsertAgencyModal(onCompleted: loadData);
                  //   },
                  // );
                },
                child: const Text(
                  "Dodaj agenciju",
                  style: TextStyle(color: Colors.white),
                ),
              ),
            )
          ],
        ),
        _displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Row(children: <Widget>[
                Expanded(
                  child: DataTable(
                    columns: const [
                      DataColumn(
                          label: Text('Putovanje',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(
                          label: Text('Datum polaska',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(
                          label: Text('Datum povratka',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(label: Text('')),
                    ],
                    rows: arrangements
                        .map(
                          (arrangement) => DataRow(
                            cells: [
                              DataCell(Text(arrangement.name)),
                              DataCell(Text(DateFormat("dd.MM.yyyy")
                                  .format(arrangement.startDate))),
                              DataCell(Text(DateFormat("dd.MM.yyyy")
                                  .format(arrangement.endDate))),
                              DataCell(
                                SizedBox(
                                  width: double.infinity,
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    // Align to the end (right)
                                    children: <Widget>[
                                      IconButton(
                                          icon: const Icon(Icons.details),
                                          onPressed: () {
                                            Navigator.pushNamed(
                                                context, '/agency/details',
                                                arguments: {
                                                  'id': arrangement.id
                                                });
                                          },
                                          tooltip: 'Detalji'),
                                      IconButton(
                                          icon: const Icon(Icons.edit_document),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return Container();
                                              },
                                            );
                                          },
                                          tooltip: 'Uredi'),
                                      IconButton(
                                          icon: const Icon(Icons.delete),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return CustomConfirmationDialog(
                                                  title: "Potvrda brisanja?",
                                                  content:
                                                      "Da li ste sigurni da želite obrisati aranžman ${arrangement?.name}",
                                                  onConfirm: () async{
                                                   await _arrangementProvider?.delete(arrangement.id);
                                                  },
                                                );
                                              },
                                            );
                                          },
                                          tooltip: 'Obriši'),
                                    ],
                                  ),
                                ),
                              ),
                            ],
                          ),
                        )
                        .toList(),
                  ),
                )
              ]),
        Row(children: <Widget>[
          Expanded(
            child: Align(
              alignment: Alignment.center,
              child: Container(
                constraints: const BoxConstraints(maxWidth: 300),
                child: Visibility(
                  visible: arrangementsCount != null &&
                      arrangementsCount! > _pageSize,
                  child: NumberPaginator(
                    initialPage: 0,
                    numberPages:
                        (((arrangementsCount == 0 || arrangementsCount == null)
                                    ? 1
                                    : arrangementsCount!) /
                                (_pageSize ?? 1))
                            .ceil(),
                    onPageChange: (int index) {
                      setState(() {
                        _currentPage = index + 1;
                      });
                      loadData();
                    },
                  ),
                ),
              ),
            ),
          )
        ])
      ],
    );
  }
}
