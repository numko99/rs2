import 'package:flutter/material.dart';

class IconTextChip extends StatelessWidget {
  final IconData iconData;
  final String label;
  final Color iconColor;
  final TextStyle? labelStyle;
  final Color backgroundColor;

  IconTextChip({
    required this.iconData,
    required this.label,
    this.iconColor = Colors.white,
    this.labelStyle,
    this.backgroundColor = Colors.amber,
  });

  @override
  Widget build(BuildContext context) {
    return Chip(
      avatar: CircleAvatar(
        backgroundColor: Colors.transparent,
        child: Icon(
          iconData,
          color: iconColor,
          size: 20,
        ),
      ),
      label: Text(
        label,
        style: labelStyle ?? const TextStyle(color: Colors.white, fontSize: 10),
      ),
      backgroundColor: backgroundColor,
    );
  }
}
