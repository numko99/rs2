import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/enums/reservation_status.dart';
import 'package:iter_mobile/models/reservation.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:provider/provider.dart';

class ReservationDetailsScreen extends StatefulWidget {
  const ReservationDetailsScreen({super.key, this.reservationId});

  final String? reservationId;

  @override
  _ReservationDetailsScreenState createState() =>
      _ReservationDetailsScreenState();
}

class _ReservationDetailsScreenState extends State<ReservationDetailsScreen> {
  bool displayLoader = true;
  Reservation? reservation;
  ReservationProvider? reservationProvider;

  @override
  void initState() {
    super.initState();
    reservationProvider = context.read<ReservationProvider>();

    loadData();
  }

  Future<void> loadData() async {
    var reservationTemp =
        await reservationProvider!.getById(widget.reservationId);

      setState(() {
        reservation = reservationTemp;
        displayLoader = false;
      });
  }

  @override
  Widget build(BuildContext context) {
    return displayLoader == true
        ? const Center(child: CircularProgressIndicator())
        : Scaffold(
        body: SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    if (reservation?.arrangement.images.isNotEmpty ?? false)
                      Stack(
                        children: [
                          Image.memory(
                            base64Decode(reservation!.arrangement.images
                                .firstWhere(
                                    (image) => image.isMainImage == true)
                                .image),
                            width: double.infinity,
                            height: 150,
                            fit: BoxFit.cover,
                          ),
                          Positioned(
                            top: MediaQuery.of(context).padding.top,
                            left: 0,
                            child: SafeArea(
                              child: IconButton(
                                icon: Icon(Icons.arrow_back,
                                    color: Theme.of(context).primaryColor),
                                onPressed: () => Navigator.of(context).pop(),
                              ),
                            ),
                          ),
                        ],
                      ),
                    Card(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Card(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(reservation!.arrangement.name,
                                    style: TextStyle(
                                      fontSize: 20.0,
                                      color: Colors.grey[600],
                                      fontWeight: FontWeight.bold,
                                    )),
                                Text(reservation!.arrangement.agency.name,
                                    style: TextStyle(
                                      fontSize: 14.0,
                                      color: Colors.grey[500],
                                      fontWeight: FontWeight.bold,
                                    )),
                                const SizedBox(height: 15),
                                ListTile(
                                    leading: const Icon(Icons.calendar_month),
                                    title: Text(
                                        "${DateFormat('dd.MM.yyyy.').format(reservation!.arrangement.startDate)}${reservation!.arrangement.endDate != null ? " - ${DateFormat('dd.MM.yyyy').format(reservation!.arrangement.endDate!)}" : ""}")),
                              ],
                            ),
                          ),
                          ListTile(
                            leading: Icon(Icons.check_circle,
                                color:
                                    getColor(reservation!.reservationStatusId)),
                            title: Text(reservation!.reservationStatusName),
                            subtitle: const Text("Status rezervacije"),
                          ),
                          ListTile(
                            leading: const Icon(Icons.place),
                            title: Text(reservation!.departurePlace ?? "-"),
                            subtitle: const Text("Mjesto polaska"),
                          ),
                          ListTile(
                            leading: const Icon(Icons.hotel),
                            title: Text(reservation!
                                .arrangementPrice!.accommodationType!),
                            subtitle: const Text("Tip smještaja"),
                          ),
                          ListTile(
                            leading: const Icon(Icons.monetization_on),
                            title: Text(
                                "${reservation!.totalPaid.toInt()}/${reservation!.arrangementPrice?.price?.toInt() ?? 0} KM"),
                            subtitle: const Text("Status uplate"),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
        bottomNavigationBar: getBottomNavigationBar());
  }

  Widget? getBottomNavigationBar() {
    if (reservation!.reservationStatusId ==
        ReservationStatusEnum.pending.index + 1) {
      return BottomAppBar(
        padding: EdgeInsets.all(8.0),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            ElevatedButton(
              onPressed: () {
                // openPaylap();
              },
              child: const Text('Izvrši plaćanje',
                  style: TextStyle(color: Colors.white)),
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(backgroundColor: Colors.red[400]),
              onPressed: () async {
                final bool? confirm = await showDialog<bool>(
                  context: context,
                  builder: (BuildContext context) {
                    return AlertDialog(
                      title: const Text('Potvrda'),
                      content: const Text(
                          'Da li ste sigurni da želite otkazati rezervaciju?'),
                      actions: <Widget>[
                        TextButton(
                          onPressed: () => Navigator.of(context).pop(false),
                          child: const Text('Ne'),
                        ),
                        TextButton(
                          onPressed: () => Navigator.of(context).pop(true),
                          child: const Text('Da'),
                        ),
                      ],
                    );
                  },
                );

                if (confirm == true) {
                  await reservationProvider!.cancelReservation(reservation!.id);
                  await loadData();
                }
              },
              child: const Text('Otkaži rezervaciju',
                  style: TextStyle(color: Colors.white)),
            ),
          ],
        ),
      );
    } else if (reservation!.reservationStatusId ==
                ReservationStatusEnum.confirmed.index + 1 &&
            (reservation!.arrangement.endDate == null &&
                reservation!.arrangement.startDate.isBefore(DateTime.now())) ||
        reservation!.arrangement.endDate!.isBefore(DateTime.now())) {
      return BottomAppBar(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: ElevatedButton(
            onPressed: () => showReviewDialog(context),
            child: const Text('Ostavi recenziju',
                style: TextStyle(color: Colors.white)),
          ),
        ),
      );
    }
    return null;
  }

  void showReviewDialog(BuildContext context) {
    final _commentController = TextEditingController();
    final _formKey = GlobalKey<FormState>();
    double rating = 0.0;

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Center(child: Text('Ostavi recenziju')),
          content: Form(
            key: _formKey,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                RatingBar.builder(
                  itemSize: 30,
                  minRating: 1,
                  direction: Axis.horizontal,
                  itemCount: 5,
                  itemPadding: const EdgeInsets.symmetric(horizontal: 4.0),
                  itemBuilder: (context, _) => const Icon(
                    Icons.star,
                    color: Colors.amber,
                  ),
                  onRatingUpdate: (ratingTemp) {
                    rating = ratingTemp;
                  },
                ),
                const SizedBox(height: 5),
                TextField(
                  controller: _commentController,
                  maxLines: 5,
                  minLines: 1,
                  decoration: InputDecoration(labelText: 'Komentar'),
                )
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              child: Text('Otkaži'),
              onPressed: () {
                Navigator.of(context).pop();
              },
            ),
            TextButton(
              child: Text('Pošalji'),
              onPressed: () async {
                await reservationProvider?.addReview(
                    reservation!.id, rating.toInt());
                await loadData();
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  Color getColor(int statusId) {
    switch (statusId) {
      case 2:
      case 3:
      case 6:
        return Colors.red;
      case 4:
        return Colors.green;
      default:
        return Colors.grey;
    }
  }
}
