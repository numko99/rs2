import 'package:flutter/material.dart';

class ReservationStatus extends StatelessWidget {
  final int statusId;
  final String status;
  const ReservationStatus({
    super.key,
    required this.statusId,
    required this.status,
  });
  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(4.0),
      decoration: BoxDecoration(
        color: statusId == 1
            ? Colors.grey
            : statusId == 2 || statusId == 3 || statusId == 6
                ? Colors.red
                : Colors.green,
        borderRadius: BorderRadius.circular(4.0),
      ),
      child: Text(
        status,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }
}
