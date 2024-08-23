import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class DateTimeHelper {
  static Future<void> selectDateAndTime(
      BuildContext context, TextEditingController controller) async {
    final DateTime? pickedDate = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime.now(),
      lastDate: DateTime(2027),
    );
    if (pickedDate != null) {
      final TimeOfDay? pickedTime = await showTimePicker(
        context: context,
        initialTime: TimeOfDay.now(),
      );
      if (pickedTime != null) {
        final DateTime pickedDateTime = DateTime(
          pickedDate.year,
          pickedDate.month,
          pickedDate.day,
          pickedTime.hour,
          pickedTime.minute,
        );
        controller.text = DateFormat('dd-MM-yyyy HH:mm').format(pickedDateTime);
      }
    }
  }

 static String formatDate(DateTime? date, String format) {
  if (date == null){
    return "";
  }

    final DateFormat formatter = DateFormat(format);
    return formatter.format(date);
  }
}
