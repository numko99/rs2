import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/reservation_status.dart';
import 'package:ui/models/reservation_search_respose.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/widgets/reservation_status.dart';

class ArrangementReservationsModal extends StatefulWidget {
  const ArrangementReservationsModal({super.key, this.arrangementId});

  final String? arrangementId;
  @override
  ArrangementReservationsModalState createState() =>
      ArrangementReservationsModalState();
}

class ArrangementReservationsModalState
    extends State<ArrangementReservationsModal> {
  ReservationProvider? _reservationProvider;
  bool _displayLoader = true;
  List<ReservationSearchResponse>? reservations;
  @override
  void initState() {
    super.initState();
    _reservationProvider = context.read<ReservationProvider>();

    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });

    var searchReservations = await _reservationProvider?.get({
      "arrangementId": widget.arrangementId,
            "reservationStatusId":
          (ReservationStatusEnum.confirmed.index + 1).toString(),
    });

    setState(() {
      _displayLoader = false;
      reservations = searchReservations?.result;
    });
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Column(
        children: [Text("Pregled rezervacija"), SizedBox(height: 30)],
      ),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: _displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : SizedBox(
              width: 600.0,
              height: 600,
              child: SingleChildScrollView(
                child: Row(children: <Widget>[
                  Expanded(
                    child: DataTable(
                      columns: const [
                        DataColumn(
                            label: Text('Ime',
                                style: TextStyle(fontWeight: FontWeight.bold))),
                        DataColumn(
                            label: Text('Prezime',
                                style: TextStyle(fontWeight: FontWeight.bold))),
                        DataColumn(
                            label: Text('Status',
                                style: TextStyle(fontWeight: FontWeight.bold))),
                      ],
                      rows: reservations!
                          .map(
                            (reservation) => DataRow(
                              cells: [
                                DataCell(Text(reservation.firstName)),
                                DataCell(Text(reservation.lastName)),
                                DataCell(ReservationStatus(statusId: reservation.reservationStatusId, status: reservation.reservationStatusName))
                              ],
                            ),
                          )
                          .toList(),
                    ),
                  ),
                ]),
              )),
    );
  }
}
