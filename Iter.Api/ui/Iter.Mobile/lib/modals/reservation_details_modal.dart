import 'package:flutter/material.dart';
import 'package:iter_mobile/models/reservation.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:provider/provider.dart';

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
          Icon(Icons.person, color: Colors.amber, size: 50),
          Text(
            "${reservation?.user?.firstName} ${reservation?.user?.lastName}",
          ),
          const SizedBox(height: 30)
        ],
      ),
      contentPadding: const EdgeInsets.fromLTRB(10.0, 5, 10.0, 20),
      content: _displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : SizedBox(
              width: 700.0,
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    ListTile(
                      leading: Icon(Icons.email, color: Colors.amber),
                      title: Text(
                        reservation!.user!.email!,
                      ),
                    ),
                    ListTile(
                      leading: Icon(Icons.phone, color: Colors.amber),
                      title: Text(
                        reservation!.user!.phoneNumber!,
                      ),
                    ),
                    if (reservation?.arrangementPrice!.accommodationType != null)
                    ListTile(
                      leading: Icon(Icons.hotel, color: Colors.amber),
                      title: Text(
                        "${reservation?.arrangementPrice!.accommodationType}",
                      ),
                    ),
                    ListTile(
                      leading: Icon(Icons.location_city, color: Colors.amber),
                      title: Text(
                        "${reservation?.departurePlace}",
                      ),
                    ),
                  ],
                ),
              )),
    );
  }
}
