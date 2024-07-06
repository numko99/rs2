import 'package:flutter/material.dart';
import 'package:ui/widgets/Layout/layout.dart';

class Home extends StatefulWidget {
  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  @override
  Widget build(BuildContext context) {
    return Layout(
            name: "Početna",
      icon: Icons.home,
      body: Container(
        child: Text("Ovo je neki tekst"),
      ),
    );
  }
}
