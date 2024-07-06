

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:iter_mobile/models/arrangement_search_response.dart';
import 'package:iter_mobile/pages/arrangement_details.dart';

class ArrangementRecommendationCard extends StatelessWidget {
  const ArrangementRecommendationCard({
    super.key,
    required this.arrangement,
  });

  final ArrangementSearchResponse arrangement;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () async =>{
        await Navigator.of(context).push(
            MaterialPageRoute(
                builder: (context) =>
                    ArrangementDetailsPage(id: arrangement.id, isReserved: arrangement.isReserved)),
          )
      },
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                arrangement.name,
                style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
              ),
              Text(
                arrangement.agencyName,
                style: TextStyle(fontSize: 12),
              ),
              SizedBox(height: 8.0),
              Image.memory(
                base64Decode(arrangement.mainImage.image),
                width: double.infinity,
                height: 120,
                fit: BoxFit.cover,
              ),
              SizedBox(height: 8.0),
              Text(
                  "Od ${arrangement.minPrice} KM")
            ],
          ),
        ),
      ),
    );
  }
}
