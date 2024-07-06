import 'package:flutter/material.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/reservation/reservation_mini_data_table.dart';

class ReservationIndexPage extends StatefulWidget {
  const ReservationIndexPage({super.key});

  @override
  State<ReservationIndexPage> createState() => _ReservationIndexPageState();
}

class _ReservationIndexPageState extends State<ReservationIndexPage> {
  String? agencyId;
  @override
  void initState() {
    super.initState();

    agencyId = AuthStorageProvider.getAuthData()?["agencyId"];
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Rezervacije",
      icon: Icons.list_alt,
      body: Card(
          child: Padding(
        padding: const EdgeInsets.all(32.0),
        child: ReservationDataTable(agencyId: agencyId),
      )),
    );
  }
}
