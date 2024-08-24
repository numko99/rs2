import 'package:flutter/material.dart';
import 'package:ui/widgets/layout.dart';
import 'package:ui/widgets/arrangement_data_table.dart';

class ArrangementIndexPage extends StatefulWidget {
  const ArrangementIndexPage({super.key});

  @override
  State<ArrangementIndexPage> createState() => _ArrangementIndexPageState();
}

class _ArrangementIndexPageState extends State<ArrangementIndexPage> {
  String? agencyId;
  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Aranžmani",
      displayBackNavigationArrow: false,
      icon: Icons.beach_access_outlined,
      body: const Card(child: Padding(
        padding: EdgeInsets.all(32.0),
        child: ArrangementDataTable(agencyId: null),
      )),
    );
  }
}
