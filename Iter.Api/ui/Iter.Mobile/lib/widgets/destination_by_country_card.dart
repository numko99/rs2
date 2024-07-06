import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/models/destination.dart'; // Pretpostavka modela

class DestinationsByCountryCard extends StatelessWidget {
  final List<Destination> destinations;

  const DestinationsByCountryCard({Key? key, required this.destinations})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    Map<String, List<Destination>> groupedByCountry = {};
    for (var destination in destinations) {
      groupedByCountry
          .putIfAbsent(destination.country!, () => [])
          .add(destination);
    }

    // Kreiranje liste Widget-a za Wrap
    List<Widget> countryWidgets = groupedByCountry.entries.map((entry) {
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(entry.key,
                  style: const TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.bold)),
              const SizedBox(width: 5),
              const Icon(Icons.location_on_rounded, color: Colors.amber),
            ],
          ),
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: entry.value
                .map((dest) => Padding(
                  padding: const EdgeInsets.fromLTRB(0, 4, 0, 2),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(dest.city!,
                          style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey[600])),
                      Text(
                          "${DateFormat('dd.MM.yyyy').format(dest.arrivalDate!)}${dest.accommodation != null ? " - ${DateFormat('dd.MM.yyyy').format(dest.departureDate!)}" : ""}",
                          style: TextStyle(color: Colors.grey[600], fontSize: 10),
                          )
                    ],
                  ),
                ))
                .toList(),
          ),
        ],
      );
    }).toList();

    return SingleChildScrollView(
      child: Wrap(
        spacing: 20.0,
        runSpacing: 4.0,
        alignment: WrapAlignment.start,
        children: countryWidgets,
      ),
    );
  }
}
