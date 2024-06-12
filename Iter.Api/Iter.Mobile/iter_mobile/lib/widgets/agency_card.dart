import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:iter_mobile/models/agency.dart';
import 'package:iter_mobile/pages/home.dart';

class AgencyCard extends StatelessWidget {
  final Agency agency;

  const AgencyCard({super.key, required this.agency});

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.white
      ),
        onPressed: () { 
          Navigator.of(context).push(
        MaterialPageRoute(
          builder: (context) => HomePage(),
        ),
      );
         },
        child: Padding(
          padding: EdgeInsets.fromLTRB(0,8,0,8),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text("AGENCIJA",
                  style: TextStyle(
                      fontSize: 12,
                      color: Colors.grey[600],
                      fontWeight: FontWeight.bold)),
              Divider(
                height: 10,
                color: Colors.grey[300],
                thickness: 1, // Debljina linije
              ),
              SizedBox(height: 6),
              Row(
                children: [
                  // Slika agencije
                  CircleAvatar(
                    radius: 30.0,
                    backgroundColor: Colors.transparent,
                    child: Image.memory(base64Decode(agency.logo?.image),
                        fit: BoxFit.cover),
                  ),
                  SizedBox(width: 10),
                  // Tekstualni podaci
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          agency.name,
                          style: TextStyle(
                            fontSize: 18,
                          ),
                        ),
                        Text(
                          agency.address!.city!,
                          style: TextStyle(
                            fontSize: 14,
                            color: Colors.grey[600],
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
