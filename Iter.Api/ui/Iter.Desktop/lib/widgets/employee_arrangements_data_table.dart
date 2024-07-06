import 'package:flutter/material.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/employee_arrangement.dart';
import 'package:ui/services/employee_arrangment_provider.dart';
import 'package:ui/widgets/search_button.dart';

class EmployeeArrangementsDataTable extends StatefulWidget {
  const EmployeeArrangementsDataTable({super.key, this.employeeId});

  final String? employeeId;

  @override
  State<EmployeeArrangementsDataTable> createState() =>
      EmployeeArrangementsDataTableState();
}

class EmployeeArrangementsDataTableState
    extends State<EmployeeArrangementsDataTable> {
  TextEditingController searchController = TextEditingController();

  EmployeeArrangmentProvider? _employeeArrangementProvider;

  List<EmployeeArrangment> reservation = [];
  int? reservationCount;

  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 10;

  @override
  void initState() {
    super.initState();
    _employeeArrangementProvider = context.read<EmployeeArrangmentProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });
    try {
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
    var searchReservation = await _employeeArrangementProvider?.get({
      "currentPage": _currentPage,
      "pageSize": _pageSize,
      "employeeId": widget.employeeId,
      "returnAll": true,
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
            const SizedBox(width: 20),
            SearchButton(
              onSearch: search,
            ),
            Spacer(),
            Spacer()
          ],
        ),
        SizedBox(height: 20),
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
                          label: Text('Ocjena',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(
                          label: Text('Status',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                    ],
                    rows: reservation
                        .map(
                          (reservation) => DataRow(
                            cells: [
                              DataCell(Text(reservation.arrangement!.name)),
                              DataCell(Text(
                                  "${DateTimeHelper.formatDate(reservation.arrangement!.startDate, "dd.MM.yyyy")}")),
                              DataCell(Text(
                                  reservation.arrangement!.rating.toString())),
                              DataCell(DateTime.now().isAfter(
                                      reservation.arrangement!.startDate)
                                  ? Chip(label: Text("Završeno"))
                                  : Chip(
                                      backgroundColor: Colors.green,
                                      label: Text("U pripremi",
                                          style:
                                              TextStyle(color: Colors.white))))
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
}
