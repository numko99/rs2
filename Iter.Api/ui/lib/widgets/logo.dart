import 'package:flutter/material.dart';

class Logo extends StatelessWidget {
  final double fontSize;
  final Color color;

  Logo({
    required this.fontSize,
    required this.color,
  });
  @override
  Widget build(BuildContext context) {
    return Text(
      'ITer',
      style: TextStyle(
        fontSize: fontSize,
        fontWeight: FontWeight.bold,
        fontFamily: 'Niconne-Regular',
        color: color,
      ),
    );
  }
}