import 'package:flutter/material.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/customConfirmationModal.dart';
import 'package:ui/modals/details_reservation_modal.dart';
import 'package:ui/modals/update_reservation_modal.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/reservation_search_respose.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/widgets/reservation_status.dart';

import '../search_button.dart';

class ReservationDataTable extends StatefulWidget {
  const ReservationDataTable({super.key, this.userId, this.agencyId});

  final String? userId;
  final String? agencyId;

  @override
  State<ReservationDataTable> createState() => ReservationDataTableState();
}

class ReservationDataTableState extends State<ReservationDataTable> {
  TextEditingController searchController = TextEditingController();

  ReservationProvider? _reservationProvider;
  DropdownProvider? _dropdownProvider;

  List<ReservationSearchResponse> reservation = [];
  int? reservationCount;

  String? selectedAgency;
  String? selectedReservationStatus;
  List<DropdownModel>? agenciesDropdown;
  List<DropdownModel>? reservationStatusDropdown;

  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 10;

  @override
  void initState() {
    super.initState();
    _reservationProvider = context.read<ReservationProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    if (widget.agencyId != null){
      selectedAgency = widget.agencyId;
    }

    setState(() {
      _displayLoader = true;
    });
    try {
      await search();

      var agenciesDropdownTemp = await _dropdownProvider!
          .get({"dropdownType": DropdownTypes.agencies});
      agenciesDropdownTemp.result
          .insert(0, new DropdownModel(id: null, name: "Nije odabrano"));
      if (agenciesDropdownTemp.result.isNotEmpty) {
        setState(() {
          agenciesDropdown = agenciesDropdownTemp.result;
        });
      }

      var reservationStatusDropdownTemp = await _dropdownProvider!
          .get({"dropdownType": DropdownTypes.reservationStatus.index});
      reservationStatusDropdownTemp.result
          .insert(0, new DropdownModel(id: null, name: "Nije odabrano"));
      if (reservationStatusDropdownTemp.result.isNotEmpty) {
        setState(() {
          reservationStatusDropdown = reservationStatusDropdownTemp.result;
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

  Future<void> search() async {
    var searchReservation = await _reservationProvider?.get({
      "currentPage": _currentPage,
      "pageSize": _pageSize,
      "agencyId": selectedAgency,
      "reservationStatusId": selectedReservationStatus,
      "userId": widget.userId,
      "name": searchController.text,
    });

    if (searchReservation != null) {
      setState(() {
        reservation = searchReservation.result;
        reservationCount = searchReservation.count;
      });
    } else {
      setState(() {
        reservation = [];
        reservationCount = 0;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: <Widget>[
            Expanded(
              flex: 1,
              child: TextFormField(
                controller: searchController,
                decoration: const InputDecoration(
                  labelText: 'Unesite pretragu',
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
            ],
              const SizedBox(width: 20),
              Expanded(
                flex: 1,
                child: DropdownButtonFormField<dynamic>(
                  decoration: const InputDecoration(
                    labelText: 'Izaberite status',
                  ),
                  value: selectedReservationStatus,
                  items: reservationStatusDropdown?.map((DropdownModel item) {
                    return DropdownMenuItem<dynamic>(
                      value: item.id,
                      child: Text(item.name ?? ""),
                    );
                  }).toList(),
                  onChanged: (value) {
                    setState(() {
                      selectedReservationStatus = value;
                    });
                  },
                ),
              ),
            const SizedBox(width: 20),
            SearchButton(
              onSearch: search,
            ),
            if (widget.userId == null)
            ...[
            const Spacer(),
            const Spacer()
            ]
          ],
        ),
        SizedBox(height: 20),
        _displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Row(children: <Widget>[
                Expanded(
                  child: DataTable(
                    columns: [
                      if (widget.agencyId == null)
                      const DataColumn(
                          label: Text('Broj rezervacije',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      if (widget.userId == null)
                      const DataColumn(
                          label: Text('Ime i prezime',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(
                          label: Text('Putovanje',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      if (widget.agencyId == null)
                      const DataColumn(
                          label: Text('Agencija',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(
                          label: Text('Status',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      const DataColumn(label: Text('')),
                    ],
                    rows: reservation
                        .map(
                          (reservation) => DataRow(
                            cells: [
                              if (widget.agencyId == null)
                              DataCell(Text(reservation.reservationNumber)),
                              if (widget.userId == null)
                              DataCell(Text("${reservation.firstName} ${reservation.lastName}")),
                              DataCell(Text(reservation.arrangementName)),
                              if (widget.agencyId == null)
                              DataCell(Text(reservation.agencyName)),
                              DataCell(
                                ReservationStatus(statusId: reservation.reservationStatusId, status: reservation.reservationStatusName)),
                              DataCell(
                                SizedBox(
                                  width: double.infinity,
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    children: <Widget>[
                                      IconButton(
                                          icon: const Icon(Icons.open_in_new_off),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return ReservationDetailsModal(
                                                    reservationId: reservation.reservationId);
                                              },
                                            );
                                          },
                                          tooltip: 'Detalji'),
                                      IconButton(
                                          icon: const Icon(Icons.edit_document),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return UpdateReservationModal(
                                                    onCompleted: () => search(),
                                                    reservationId: reservation.reservationId);
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
                                                      "Da li ste sigurni da želite obrisati rezervaciju ${reservation.reservationNumber}?",
                                                  onConfirm: () async {
                                                    await delete(reservation.reservationId);
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
                  visible:
                      reservationCount != null && reservationCount! > _pageSize,
                  child: NumberPaginator(
                    initialPage: 0,
                    numberPages:
                        (((reservationCount == 0 || reservationCount == null)
                                    ? 1
                                    : reservationCount!) /
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

  Future<void> delete(id) async {
    try {
      setState(() {
        _displayLoader = true;
      });
      await _reservationProvider?.delete(id);
      search();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste obrisali rezervaciju",
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

