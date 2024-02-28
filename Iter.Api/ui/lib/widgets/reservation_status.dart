import 'package:flutter/material.dart';

class reservationStatus extends StatelessWidget {
  final int statusId;
  final String status;
  reservationStatus({
    required this.statusId,
    required this.status,
  });
  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(8.0),
      color: statusId == 1
          ? Colors.grey
          : statusId == 2 || statusId == 3 || statusId == 6
              ? Colors.red
              : Colors.green,
      child: Text(
        status,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }
}
