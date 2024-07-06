import 'package:flutter/material.dart';

class ReservationStatus extends StatelessWidget {
  final int statusId;
  final String status;
  ReservationStatus({
    required this.statusId,
    required this.status,
  });
  @override
  Widget build(BuildContext context) {
    return Chip(
      padding: const EdgeInsets.all(8.0),
      backgroundColor: statusId == 1
          ? Colors.grey
          : statusId == 2 || statusId == 3 || statusId == 6
              ? Colors.red
              : Colors.green,
      label: Text(
        status,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }
}
