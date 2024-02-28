import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/models/reservation.dart';
import 'package:ui/services/reservation_provider.dart';

class ReservationDetailsModal extends StatefulWidget {
  const ReservationDetailsModal({super.key, this.reservationId});

  final String? reservationId;
  @override
  ReservationDetailsModalState createState() => ReservationDetailsModalState();
}

class ReservationDetailsModalState extends State<ReservationDetailsModal> {
  ReservationProvider? _reservationProvider;
  bool _displayLoader = true;
  Reservation? reservation;

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

    var reservationTemp =
        await _reservationProvider?.getById(widget.reservationId);

    setState(() {
      _displayLoader = false;
      reservation = reservationTemp;
    });
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Column(
        children: [
          Text("Pregled rezervacije ${reservation?.reservationNumber}"),
          const SizedBox(height: 30)
        ],
      ),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: _displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : SizedBox(
              width: 600.0,
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    Row(
                      children: [
                        Expanded(
                          flex: 1,
                          child: Column(
                            children: [
                              const Icon(
                                Icons.person,
                                color: Colors.amber,
                                size: 40.0,
                              ),
                              Text(
                                "${reservation?.user?.firstName} ${reservation?.user?.lastName}",
                                style: const TextStyle(
                                  fontSize: 18.0,
                                ),
                              ),
                            ],
                          ),
                        ),
                        Expanded(
                          flex: 1,
                          child: Column(
                            children: [
                              Icon(
                                Icons.list_alt,
                                color: getColor(),
                                size: 40.0,
                              ),
                              Text(
                                reservation?.reservationStatusName ?? "",
                                style: TextStyle(
                                  color: getColor(),
                                  fontSize: 18.0,
                                ),
                              ),
                            ],
                          ),
                        ),
                        Expanded(
                          flex: 1,
                          child: Column(
                            children: [
                              const Icon(
                                Icons.beach_access_outlined,
                                color: Colors.amber,
                                size: 40.0,
                              ),
                              Text(
                                reservation!.arrangement.name,
                                style: const TextStyle(
                                  fontSize: 18.0,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 30),
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Flexible(
                          child: ListTile(
                            leading: const Tooltip(
                              message: 'Agencija',
                              child: Icon(Icons.bus_alert_outlined,
                                  color: Colors.amber),
                            ),
                            title: Text(
                                reservation?.arrangement.agency.name ?? ""),
                          ),
                        ),
                        Flexible(
                          child: ListTile(
                            leading: const Tooltip(
                              message: 'Mjesto polaska',
                              child: Icon(Icons.location_city,
                                  color: Colors.amber),
                            ),
                            title: Text(reservation?.departurePlace ?? ""),
                          ),
                        ),
                      ],
                    ),
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Flexible(
                          child: ListTile(
                            leading: const Tooltip(
                              message: 'Tip smještaja',
                              child: Icon(Icons.hotel, color: Colors.amber),
                            ),
                            title: Text(reservation
                                    ?.arrangementPrice?.accommodationType ??
                                ""),
                          ),
                        ),
                        Flexible(
                          child: ListTile(
                            leading: const Tooltip(
                              message: 'Uplaćeno',
                              child: Icon(Icons.payment, color: Colors.amber),
                            ),
                            title: Text(
                                "${reservation?.totalPaid.toInt()}/${reservation?.arrangementPrice!.price?.toInt()} KM"),
                          ),
                        ),
                      ],
                    ),
                    // Row(
                    //   crossAxisAlignment: CrossAxisAlignment.start,
                    //   children: [
                    //     Expanded(child: Text(reservation?.reminder ?? ""))
                    //   ],
                    // ),
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Flexible(
                          child: ListTile(
                            leading: const Tooltip(
                              message: 'Datum i vrijeme prijave',
                              child: Icon(Icons.calendar_month,
                                  color: Colors.amber),
                            ),
                            title: Text(DateTimeHelper.formatDate(
                                DateTime.now(), "dd.MM.yyyy HH:mm")),
                          ),
                        ),
                        const Flexible(
                          child: ListTile(
                            leading: Tooltip(
                              message: 'Ocjena',
                              child: Icon(Icons.star, color: Colors.amber),
                            ),
                            title: Text("TODO"),
                          ),
                        )
                      ],
                    ),
                    const SizedBox(height: 30),
                    Row(
                      children: [
                        Flexible(
                          child: ListTile(
                            leading: const Tooltip(
                              message: 'Napomena',
                              child: Icon(Icons.note, color: Colors.amber),
                            ),
                            title: Text(reservation?.reminder ?? ""),
                          ),
                        )
                      ],
                    )
                  ],
                ),
              )),
    );
  }

  MaterialColor getColor() {
    return reservation!.reservationStatusId == 2 ||
            reservation!.reservationStatusId == 3 ||
            reservation!.reservationStatusId == 6
        ? Colors.red
        : reservation!.reservationStatusId == 4
            ? Colors.green
            : Colors.grey;
  }
}
