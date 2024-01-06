import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class CustomConfirmationDialog extends StatelessWidget {
  final String title;
  final String content;
  final VoidCallback onConfirm;

  const CustomConfirmationDialog({
    Key? key,
    required this.title,
    required this.content,
    required this.onConfirm,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text(title),
      content: Text(content),
      actions: [
        ElevatedButton(
          style: ButtonStyle(
            backgroundColor: MaterialStateProperty.all(Colors.white70),
          ),
          onPressed: () => Navigator.of(context).pop(),
          child: const Text("Odustani"),
        ),
        ElevatedButton(
          style: ButtonStyle(
            backgroundColor: MaterialStateProperty.all(Colors.red),
          ),
          onPressed: () {
            onConfirm();
            Navigator.of(context).pop();
          },
          child: const Text(
            "Potvrdi",
            style: TextStyle(color: Colors.white),
          ),
        ),
      ],
    );
  }
}
