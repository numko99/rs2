import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:iter_mobile/enums/roles.dart';
import 'package:iter_mobile/pages/arrangement_details.dart';
import 'package:iter_mobile/pages/employee_arrangement_details.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';

class ArrangementCard extends StatelessWidget {
  final String id;
  final String image;
  final String name;
  final String departureDate;
  final String agencyName;
  final String? agencyRating;
  final bool? isReserved;

  const ArrangementCard(
      {super.key,
      required this.image,
      required this.id,
      required this.name,
      required this.departureDate,
      required this.agencyName,
      this.agencyRating,
      this.isReserved});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
        onTap: () {
          Navigator.of(context).push(
            MaterialPageRoute(
                builder: (context) => AuthStorageProvider.getAuthData()?["role"] == Roles.client
                                    ? ArrangementDetailsPage(id: id, isReserved: isReserved)
                                    : EmployeeArrangementDetailsPage(id: id)
                                    ),
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
              Stack(children: [
                ClipRRect(
                  borderRadius:
                      const BorderRadius.vertical(top: Radius.circular(10.0)),
                  child: Image.memory(
                    base64Decode(image),
                    width: double.infinity,
                    height: 150,
                    fit: BoxFit.cover,
                  ),
                ),
                if (isReserved == true)
                  const Positioned(
                      right: 5,
                      top: 5,
                      child: Chip(
                        label: Text(
                          "Rezervisano",
                          style: TextStyle(color: Colors.white, fontSize: 10),
                        ),
                        backgroundColor: Colors.red,
                      ))
              ]),
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Center(
                      child: Text(
                        name,
                        style: const TextStyle(
                          fontSize: 20.0,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                    const SizedBox(height: 5.0),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              agencyName,
                              style: const TextStyle(fontSize: 16.0),
                            ),
                            if (agencyRating != null)
                            Row(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                Text(
                                  agencyRating ?? "",
                                  style: const TextStyle(fontSize: 16.0),
                                ),
                                Icon(
                                  Icons.star,
                                  color: Colors.yellow[700],
                                  size: 18.0,
                                ),
                              ],
                            ),
                          ],
                        ),
                        Text(
                          departureDate,
                          style: TextStyle(
                            fontSize: 16.0,
                            color: Colors.grey[600],
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ],
          ),
        ));
  }
}
