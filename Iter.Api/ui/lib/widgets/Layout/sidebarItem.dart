import 'package:flutter/material.dart';

class SidebarItem extends StatelessWidget {
  final String text;
  final IconData icon;
  final String link;

  SidebarItem({
    required this.text,
    required this.icon,
    required this.link,
  });

  @override
  Widget build(BuildContext context) {
    return ListTile(
      leading: Icon(
        icon,
        color: Colors.amber,
        size: 25.0,
      ),
      title: Text(
        text,
        style: TextStyle(
          color: Colors.amber,
          fontSize: 20.0,
        ),
        // textAlign: TextAlign.center,
      ),
      onTap: () {
        Navigator.pushReplacementNamed(context, link);
      },
    );
  }
}
