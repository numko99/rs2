import 'package:flutter/material.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/Agency/insertAgencyModal.dart';
import 'package:ui/models/agency.dart';
import 'package:ui/services/agency_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/search_button.dart';

import '../modals/customConfirmationModal.dart';

class AgencyPage extends StatefulWidget {
  const AgencyPage({super.key});

  @override
  State<AgencyPage> createState() => _AgencyPageState();
}

class _AgencyPageState extends State<AgencyPage> {
  AgencyProvider? _agencyProvider;
  List<Agency> agencies = [];
  int? agencyCount;
  bool displayLoader = true;

  int _currentPage = 1;
  int pageSize = 3;
  TextEditingController searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _agencyProvider = context.read<AgencyProvider>();
    loadData();
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
                  name: "Agencije",
      icon: Icons.business,
      body: Card(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
          child: Column(
            children: <Widget>[
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
                        showDialog(
                          context: context,
                          builder: (BuildContext context) {
                            return InsertAgencyModal(onCompleted: loadData);
                          },
                        );
                      },
                      child: const Text(
                        "Dodaj agenciju",
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
                  )
                ],
              ),
              const SizedBox(height: 50),
              Column(
                children: [
                  if (displayLoader)
                    const CircularProgressIndicator()
                  else
                    Row(children: <Widget>[
                      Expanded(
                        child: DataTable(
                          columns: const [
                            DataColumn(
                                label: Text('Naziv',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Grad',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Email',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Kontakt telefon',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(label: Text('')),
                          ],
                          rows: agencies
                              .map(
                                (agency) => DataRow(
                                  cells: [
                                    DataCell(Text(agency.name)),
                                    DataCell(Text(agency.address!.city!)),
                                    DataCell(Text(agency.contactEmail)),
                                    DataCell(Text(agency.contactPhone)),
                                    DataCell(
                                      SizedBox(
                                        width: double.infinity,
                                        child: Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.end,
                                          // Align to the end (right)
                                          children: <Widget>[
                                            IconButton(
                                                icon: const Icon(Icons.open_in_new_off),
                                                onPressed: () {
                                                  Navigator.pushNamed(context,
                                                      '/agency/details',
                                                      arguments: {
                                                        'id': agency.id
                                                      });
                                                },
                                                tooltip: 'Detalji'),
                                            IconButton(
                                                icon: const Icon(
                                                    Icons.edit_document),
                                                onPressed: () {
                                                  showDialog(
                                                    context: context,
                                                    builder:
                                                        (BuildContext context) {
                                                      return InsertAgencyModal(
                                                          onCompleted: loadData,
                                                          agency: agency);
                                                    },
                                                  );
                                                },
                                                tooltip: 'Uredi'),
                                            IconButton(
                                                icon: const Icon(Icons.delete),
                                                onPressed: () {
                                                  showDialog(
                                                    context: context,
                                                    builder:
                                                        (BuildContext context) {
                                                      return CustomConfirmationDialog(
                                                        title:
                                                            "Potvrda brisanja?",
                                                        content:
                                                            "Da li ste sigurni da želite obrisati agenciju ${agency?.name}",
                                                        onConfirm: () async {
                                                          await _agencyProvider
                                                              ?.delete(
                                                                  agency.id);
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
                      ),
                    ]),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      Expanded(
                        child: Align(
                          alignment: Alignment.center,
                          child: Container(
                            constraints: const BoxConstraints(maxWidth: 300),
                            child: Visibility(
                              visible: agencyCount != null && agencyCount! > pageSize,
                              child: NumberPaginator( //TODO FIX NUMBER PAGINATOR
                                initialPage: 0,
                                numberPages:
                                    (((agencyCount == 0 || agencyCount == null)
                                                ? 1
                                                : agencyCount!) /
                                            (pageSize ?? 1))
                                        .ceil(),
                                onPageChange: (int index) {
                                  loadPage(index, pageSize);
                                },
                              ),
                            ),
                          ),
                        ),
                      )
                    ],
                  )
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> loadData() async {
    try {
      setState(() {
        displayLoader = true;
      });
      var searchResultAgencies = await _agencyProvider?.get({
        "currentPage": _currentPage,
        "pageSize": pageSize,
        "name": searchController?.text
      });

      if (searchResultAgencies != null) {
        setState(() {
          agencies = searchResultAgencies.result;
          agencyCount = searchResultAgencies.count;
          displayLoader = false;
        });
      } else {
        setState(() {
          agencies = [];
          agencyCount = 0;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }

  Future<void> loadPage(int page, int? pageSize) async {
    setState(() {
      _currentPage = page + 1;
    });
    await loadData();
  }
}
