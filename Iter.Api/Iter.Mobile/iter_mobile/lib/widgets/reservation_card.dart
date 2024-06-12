import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/enums/reservation_status.dart';
import 'package:iter_mobile/models/reservation_search_respose.dart';
import 'package:iter_mobile/pages/reservation_details_page.dart';
import 'package:iter_mobile/widgets/reservation_status.dart';

class ReservationCard extends StatelessWidget {
  final ReservationSearchResponse reservation;

  const ReservationCard({
    super.key,
    required this.reservation,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
        onTap: () {
          Navigator.of(context).push(
            MaterialPageRoute(
                builder: (context) => ReservationDetailsScreen(
                    reservationId: reservation.reservationId)),
          );
        },
        child: Card(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10.0),
          ),
          elevation: 5.0,
          margin: const EdgeInsets.all(8.0),
          child: Column(
            children: [
              ClipRRect(
                borderRadius:
                    const BorderRadius.vertical(top: Radius.circular(10.0)),
                child: Image.memory(
                  base64Decode(reservation.mainImage.image),
                  width: double.infinity,
                  height: 150,
                  fit: BoxFit.cover,
                ),
              ),
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          reservation.arrangementName,
                          style: const TextStyle(
                            fontSize: 20.0,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        (reservation.arrangementStartDate.isBefore(DateTime.now())
                                && reservation.reservationStatusId == ReservationStatusEnum.confirmed.index + 1)
                            ? ReservationStatus(
                                statusId: reservation.reservationStatusId,
                                status: "Završeno")
                            : ReservationStatus(
                                statusId: reservation.reservationStatusId,
                                status: reservation.reservationStatusName)
                      ],
                    ),
                    const SizedBox(height: 5.0),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          reservation.agencyName,
                          style: const TextStyle(fontSize: 16.0),
                        ),
                        Text(
                          DateFormat('dd.MM.yyyy')
                              .format(reservation.arrangementStartDate),
                          style: TextStyle(
                            fontSize: 16.0,
                            color: Colors.grey[600],
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 5.0),
                    if (reservation.reservationStatusId ==
                        ReservationStatusEnum.confirmed.index + 1)
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Row(
                            children: [
                              const Text("Uplaćeno:"),
                              const SizedBox(width: 5),
                              Text(
                                  "${reservation.totalPaid.round()}/${reservation.arrangementPrice.round()} KM",
                                  style: const TextStyle(
                                      fontWeight: FontWeight.bold)),
                            ],
                          )
                        ],
                      )
                  ],
                ),
              ),
            ],
          ),
        ));
  }
}
