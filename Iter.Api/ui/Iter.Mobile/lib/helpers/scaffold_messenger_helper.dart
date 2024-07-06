import 'package:flutter/material.dart';

class ScaffoldMessengerHelper {
  static void showCustomSnackBar({
    required BuildContext context,
    required String message,
    required Color backgroundColor,
  }) {

    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      content: Center(child: Text(message)),
      backgroundColor: backgroundColor,
    ));
  }
}
