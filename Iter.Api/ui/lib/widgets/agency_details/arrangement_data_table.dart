import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:intl/intl.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/Agency/insertAgencyModal.dart';
import 'package:ui/modals/insert_reservation_modal.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/dropdown_provider.dart';

import '../../modals/customConfirmationModal.dart';
import '../../models/arrangment.dart';
import '../../services/arrangment_provider.dart';
import '../search_button.dart';

class ArrangementDataTable extends StatefulWidget {
  const ArrangementDataTable({super.key, this.agencyId});

  final String? agencyId;

  @override
  State<ArrangementDataTable> createState() => _ArrangementDataTableState();
}

class _ArrangementDataTableState extends State<ArrangementDataTable> {
  var dateFormKey = GlobalKey<FormBuilderState>();
  TextEditingController searchController = TextEditingController();

  ArrangmentProvider? _arrangementProvider;
  DropdownProvider? _dropdownProvider;

  DateTime? dateFrom;
  DateTime? dateTo;
  String? selectedAgency;
  List<DropdownModel>? agenciesDropdown;

  List<Arrangement> arrangements = [];
  int? arrangementsCount;

  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 5;

  @override
  void initState() {
    super.initState();
    _arrangementProvider = context.read<ArrangmentProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    try {
      if (widget.agencyId == null) {
        var agenciesDropdownTemp = await _dropdownProvider!
            .get({"dropdownType": DropdownTypes.agencies});
        agenciesDropdownTemp.result
            .insert(0, new DropdownModel(id: null, name: "Nije odabrano"));
        if (agenciesDropdownTemp.result.isNotEmpty) {
          setState(() {
            agenciesDropdown = agenciesDropdownTemp.result;
          });
        }
      } else {
        setState(() {
          selectedAgency = widget.agencyId;
        });
      }
      await search();
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }

  Future<void> search() async {
    try {
      setState(() {
        _displayLoader = true;
      });
      var searchArrangements = await _arrangementProvider?.get({
        "currentPage": _currentPage,
        "pageSize": _pageSize,
        "agencyId": selectedAgency,
        "name": searchController.text,
        "dateFrom": DateTimeHelper.formatDate(dateFrom, "dd.MM.yyyy"),
        "dateTo": DateTimeHelper.formatDate(dateTo, "dd.MM.yyyy")
      });

      if (searchArrangements != null) {
        setState(() {
          arrangements = searchArrangements.result;
          arrangementsCount = searchArrangements.count;
        });
      } else {
        setState(() {
          arrangements = [];
          arrangementsCount = 0;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.start,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: <Widget>[
            Expanded(
              flex: 1,
              child: TextFormField(
                controller: searchController,
                decoration: const InputDecoration(
                  labelText: 'Naziv',
                ),
                onFieldSubmitted: (value) async {
                  await search();
                },
              ),
            ),
            if (widget.agencyId == null) ...[
              const SizedBox(width: 20),
              Expanded(
                flex: 1,
                child: DropdownButtonFormField<dynamic>(
                  decoration: const InputDecoration(
                    labelText: 'Izaberite agenciju',
                  ),
                  value: selectedAgency,
                  items: agenciesDropdown?.map((DropdownModel item) {
                    return DropdownMenuItem<dynamic>(
                      value: item.id,
                      child: Text(item.name ?? ""),
                    );
                  }).toList(),
                  onChanged: (value) {
                    setState(() {
                      selectedAgency = value;
                    });
                  },
                ),
              ),
              const SizedBox(width: 20),
              Expanded(
                flex: 2,
                child: FormBuilder(
                  key: dateFormKey,
                  child: Row(children: [
                    Expanded(
                      child: FormBuilderDateTimePicker(
                        name: 'startDate',
                        onChanged: (value) => setState(() => dateFrom = value),
                        decoration:
                            const InputDecoration(labelText: 'Datum od'),
                        inputType: InputType.date,
                      ),
                    ),
                    const SizedBox(width: 20),
                    Expanded(
                      child: FormBuilderDateTimePicker(
                        name: 'endDate',
                        onChanged: (value) =>
                            setState(() => dateTo = value), // Ispravljeno
                        decoration:
                            const InputDecoration(labelText: 'Datum do'),
                        inputType: InputType.date,
                      ),
                    ),
                  ]),
                ),
              ),
              const SizedBox(width: 20),
              ElevatedButton(
                  onPressed: () => {
                        setState(() => {
                              searchController.text = "",
                              selectedAgency = null,
                              dateFormKey.currentState?.reset()
                            })
                      },
                  child: const Text("Očisti filter",
                      style: TextStyle(color: Colors.white)))
            ],
            const SizedBox(width: 20),
            SearchButton(
              onSearch: search,
            ),
            const Spacer(),
            const Spacer(),
          ],
        ),
        Row(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
            Padding(
              padding: const EdgeInsets.only(top: 32),
              child: ElevatedButton(
                onPressed: () {
                  Navigator.pushNamed(context, '/arrangement/addEdit',
                      arguments: {'agencyId': widget.agencyId});
                },
                child: const Text(
                  "Dodaj aranžman",
                  style: TextStyle(color: Colors.white),
                ),
              ),
            ),
          ],
        ),
        _displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Row(children: <Widget>[
                Expanded(
                  child: DataTable(
                    columns: [
                      const DataColumn(
                          label: Text('Putovanje',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      if (widget.agencyId == null)
                        const DataColumn(
                            label: Text('Agencija',
                                style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(
                          label: Text('Datum polaska',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(
                          label: Text('Datum povratka',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(label: Text('')),
                    ],
                    rows: arrangements
                        .map(
                          (arrangement) => DataRow(
                            cells: [
                              DataCell(Text(arrangement.name)),
                              if (widget.agencyId == null)
                                DataCell(Text(arrangement.agency.name)),
                              DataCell(Text(DateFormat("dd.MM.yyyy")
                                  .format(arrangement.startDate))),
                              DataCell(arrangement.endDate != null
                                  ? Text(DateFormat("dd.MM.yyyy")
                                      .format(arrangement.endDate!))
                                  : const Text("")),
                              DataCell(
                                SizedBox(
                                  width: double.infinity,
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    // Align to the end (right)
                                    children: <Widget>[
                                      IconButton(
                                          icon:
                                              const Icon(Icons.open_in_new_off),
                                          onPressed: () {
                                            Navigator.pushNamed(
                                                context, '/arrangement/details',
                                                arguments: {
                                                  'id': arrangement.id,
                                                });
                                          },
                                          tooltip: 'Detalji'),
                                      IconButton(
                                          icon: const Icon(Icons.edit_document),
                                          onPressed: () {
                                            Navigator.pushNamed(
                                                context, '/arrangement/addEdit',
                                                arguments: {
                                                  'agencyId': widget.agencyId,
                                                  'arrangementId':
                                                      arrangement.id
                                                });
                                          },
                                          tooltip: 'Uredi'),
                                      IconButton(
                                          icon: const Icon(Icons.list_alt),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return InsertReservationModal(
                                                    arrangementId:
                                                        arrangement.id,
                                                    onCompleted: () =>
                                                        search());
                                              },
                                            );
                                          },
                                          tooltip: 'Dodaj rezervaciju'),
                                      IconButton(
                                          icon: const Icon(Icons.delete),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return CustomConfirmationDialog(
                                                  title: "Potvrda brisanja?",
                                                  content:
                                                      "Da li ste sigurni da želite obrisati aranžman ${arrangement.name}?",
                                                  onConfirm: () async {
                                                    await delete(
                                                        arrangement.id);
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
                      search();
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

  Future<void> delete(id) async {
    try {
      setState(() {
        _displayLoader = true;
      });
      await _arrangementProvider?.delete(id);
      search();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste obrisali aranžman",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }
}
