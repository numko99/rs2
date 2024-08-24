import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:intl/intl.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/arrangement_status.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/insert_reservation_modal.dart';
import 'package:ui/models/arrangement_search_response.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/services/dropdown_provider.dart';

import '../modals/custom_confirmation_modal.dart';
import '../services/arrangment_provider.dart';
import 'search_button.dart';

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

  List<ArrangementSearchResponse> arrangements = [];
  int? arrangementsCount;
  String? currentUserAgencyId;
  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 10;

  @override
  void initState() {
    super.initState();
    _arrangementProvider = context.read<ArrangmentProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    currentUserAgencyId = AuthStorageProvider.getAuthData()?["agencyId"];
    try {
      if (widget.agencyId == null && currentUserAgencyId == null) {
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
          selectedAgency = widget.agencyId ?? currentUserAgencyId;
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
                  setState(() {
                    _currentPage = 1;
                  });
                  await search();
                },
              ),
            ),
            if (widget.agencyId == null && currentUserAgencyId == null) ...[
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
            ],
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
                            setState(() => dateTo = value),
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
                      style: TextStyle(color: Colors.white))),
            const SizedBox(width: 20),
            SearchButton(
              onSearch: () async{
                setState(() {
                  _currentPage = 1;
                });
                await search();
              }
            ),
            if (currentUserAgencyId != null) const Spacer()
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
                      if (widget.agencyId == null && currentUserAgencyId == null)
                        const DataColumn(
                            label: Text('Agencija',
                                style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(
                          label: Text('Polazak',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      if (widget.agencyId == null)
                      const DataColumn(
                          label: Text('Povratak',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(
                          label: Text('Status prijava',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(label: Text('')),
                    ],
                    rows: arrangements
                        .map(
                          (arrangement) => DataRow(
                            cells: [
                              DataCell(Text(arrangement.name)),
                              if (widget.agencyId == null && currentUserAgencyId == null)
                                DataCell(Text(arrangement.agencyName)),
                              DataCell(Text(DateFormat("dd.MM.yyyy")
                                  .format(arrangement.startDate))),
                              if (widget.agencyId == null)
                              DataCell(arrangement.endDate != null
                                  ? Text(DateFormat("dd.MM.yyyy")
                                      .format(arrangement.endDate!))
                                  : const Text("")),
                              DataCell(
                                Chip(
                                  padding: const EdgeInsets.all(8.0),
                                  backgroundColor: arrangement.arrangementStatusId ==
                                          ArrangementStatus.inPreparation.index
                                      ? Colors.grey
                                      : arrangement.arrangementStatusId ==
                                              ArrangementStatus
                                                  .availableForReservation.index
                                          ? Colors.amber
                                          : Colors.green,
                                  label:
                                      Text(arrangement.arrangementStatusName ?? "", style: TextStyle(color: Colors.white)),
                                ),
                              ),
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
                                          onPressed: arrangement.startDate.isAfter(DateTime.now()) ? () {
                                            Navigator.pushNamed(
                                                context, '/arrangement/addEdit',
                                                arguments: {
                                                  'agencyId': widget.agencyId,
                                                  'arrangementId':
                                                      arrangement.id
                                                });
                                          } : null,
                                          tooltip: 'Uredi'),
                                      IconButton(
                                          icon: const Icon(Icons.list_alt),
                                          onPressed: arrangement.arrangementStatusId == ArrangementStatus.availableForReservation.index ? () {
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
                                          } : null,
                                          tooltip: 'Dodaj rezervaciju'),
                                      IconButton(
                                          icon: getArrangementIcon(
                                              arrangement.arrangementStatusId, arrangement.startDate),
                                          onPressed: () {
                                            if (arrangement.startDate.isAfter(DateTime.now())){
                                            showDialog(
                                                context: context,
                                                builder:
                                                    (BuildContext context) {
                                                  return getConfirmationDialog(
                                                      arrangement);
                                                });
                                            }
                                          },
                                          tooltip: arrangement
                                                      .arrangementStatusId ==
                                                  ArrangementStatus
                                                      .inPreparation.index
                                              ? 'Aktiviraj prijave'
                                              : arrangement
                                                          .arrangementStatusId ==
                                                      ArrangementStatus
                                                          .availableForReservation
                                                          .index
                                                  ? 'Zatvori prijave'
                                                  : "Reaktiviraj prijave"),
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
                constraints: const BoxConstraints(maxWidth: 400),
                child: Visibility(
                  visible: arrangementsCount != null &&
                      arrangementsCount! > _pageSize,
                  child: NumberPaginator(
                    initialPage: 0,
                    numberPages:
                        (((arrangementsCount == 0 || arrangementsCount == null)
                                    ? 1
                                    : arrangementsCount!) /
                                _pageSize)
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

  Icon getArrangementIcon(int? status, DateTime date) {
    var arrangementStatus = ArrangementStatus.values[status!];
    switch (arrangementStatus) {
      case ArrangementStatus.inPreparation:
        return const Icon(
          Icons.event_available,
          color: Colors.blue,
        );
      case ArrangementStatus.availableForReservation:
        return const Icon(
          Icons.close,
          color: Colors.green,
        );
      case ArrangementStatus.reservationsClosed:
        return Icon(
          Icons.event_available,
          color: date.isAfter(DateTime.now()) ? Colors.red : Colors.grey,
        );
      default:
        return const Icon(
          Icons.error,
          color: Colors.grey,
        );
    }
  }

CustomConfirmationDialog getConfirmationDialog(ArrangementSearchResponse arrangement) {
    String title;
    String content;
    ArrangementStatus nextStatus;

    var currentStatus =
        ArrangementStatus.values[arrangement.arrangementStatusId!];

    switch (currentStatus) {
      case ArrangementStatus.inPreparation:
        title = 'Aktivacija prijava';
        content = 'Da li ste sigurni da želite aktivirati prijave?';
        nextStatus = ArrangementStatus.availableForReservation;
        break;
      case ArrangementStatus.availableForReservation:
        title = 'Deaktivacija prijava';
        content = 'Da li ste sigurni da želite zatvoriti rezervacije?';
        nextStatus = ArrangementStatus.reservationsClosed;
        break;
      case ArrangementStatus.reservationsClosed:
        title = 'Ponovna aktivacija prijava';
        content = 'Da li ste sigurni da želite ponovo aktivirati prijave?';
        nextStatus = ArrangementStatus.availableForReservation;
        break;
      default:
        throw 'Nepoznati status aranžmana';
    }

    return CustomConfirmationDialog(
      title: title,
      content: content,
      onConfirm: () async {
        try {
          setState(() => _displayLoader = true);
          await _arrangementProvider?.changeStatus(
              arrangement.id, nextStatus.index);
          await search();
          ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message: "Uspješno ste promijenili status aranžmana",
            backgroundColor: Colors.green,
          );
        } catch (error) {
          final errorMessage = error.toString();
          ScaffoldMessengerHelper.showCustomSnackBar(
            context: context,
            message: "Došlo je do greške",
            backgroundColor: Colors.red,
          );
        } finally {
          setState(() => _displayLoader = false);
        }
      },
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
