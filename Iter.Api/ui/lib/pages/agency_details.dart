import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/models/arrangment.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/agency_details/arrangement_data_table.dart';
import 'package:ui/widgets/reservation/reservation_mini_data_table.dart';
import '../models/agency.dart';
import '../services/agency_provider.dart';
import '../services/arrangment_provider.dart';

class AgencyDetailsPage extends StatefulWidget {
  const AgencyDetailsPage({super.key, required this.id});

  final String id;

  @override
  State<AgencyDetailsPage> createState() => _AgencyDetailsPageState();
}

class _AgencyDetailsPageState extends State<AgencyDetailsPage> {
  AgencyProvider? _agencyProvider;

  Agency? agency;
  bool displayLoader = true;

  @override
  void initState() {
    super.initState();
    _agencyProvider = context.read<AgencyProvider>();
    loadData();
  }

  Future<void> loadData() async {
    setState(() {
      displayLoader = true;
    });

    var searchAgency = await _agencyProvider?.getById(widget.id);
    setState(() {
      agency = searchAgency;
      displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Agencije",
      icon: Icons.business,
      body: displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : Card(
              child: Padding(
                padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
                child: Column(
                  children: [
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Expanded(
                            flex: 2,
                            child: Container(
                              child: _buildAgencyDetails(agency),
                            )),
                        Expanded(flex: 3, child: _buildArrangmantTable()),
                      ],
                    ),
                  ],
                ),
              ),
            ),
    );
  }

  Widget _buildAgencyDetails(Agency? agency) {
    return Column(
      children: [
        const SizedBox(height: 15),
        Center(
            child: Text(
          agency!.name,
          style: const TextStyle(fontSize: 20),
        )),
        Container(
          padding: const EdgeInsets.fromLTRB(5, 5, 5, 5),
          width: 150,
          height: 150,
          child: agency.logo != null ? Image.memory(base64Decode(agency.logo?.image)) : Container() //BASE IMAGE TODO,
        ),
        const SizedBox(height: 20),
        Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Email',
                  child: Icon(Icons.email, color: Colors.amber),
                ),
                title: Text(agency?.contactEmail ?? ''),
              ),
            ),
            Flexible(
                child: ListTile(
              leading: const Tooltip(
                message: 'Broj telefona',
                child: Icon(Icons.phone, color: Colors.amber),
              ),
              title: Text(agency?.contactPhone ?? ''),
            )),
          ],
        ),
        Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Web stranica',
                  child: Icon(Icons.contact_page, color: Colors.amber),
                ),
                title: Text(agency?.website ?? ''),
              ),
            ),
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Broj licence',
                  child: Icon(Icons.badge, color: Colors.amber),
                ),
                title: Text(agency?.licenseNumber ?? ''),
              ),
            ),
          ],
        ),
        Row(
          children: [
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Država',
                  child:
                      Icon(Icons.location_city_outlined, color: Colors.amber),
                ),
                title: Text('${agency?.address?.country}'),
              ),
            ),
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Grad',
                  child: Icon(Icons.location_city, color: Colors.amber),
                ),
                title: Text('${agency?.address?.city}'),
              ),
            )
          ],
        ),
        Row(
          children: [
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Poštanski broj',
                  child: Icon(Icons.numbers, color: Colors.amber),
                ),
                title: Text('${agency.address?.postalCode}'),
              ),
            ),
            Flexible(
              child: ListTile(
                leading: const Tooltip(
                  message: 'Ulica',
                  child: Icon(Icons.home, color: Colors.amber),
                ),
                title: Text(
                    '${agency?.address?.street} ${agency.address?.houseNumber}' ??
                        ''),
              ),
            )
          ],
        )
      ],
    );
  }

  Widget _buildArrangmantTable() {
    return DefaultTabController(
      length: 2,
      child: Column(
        children: [
          SizedBox(
            width: 300,
            child: TabBar(
              tabs: [
                Tab(text: 'Putovanja'),
                Tab(text: 'Rezervacije'),
              ],
            ),
          ),
          Container(
            height: 500,
            child: TabBarView(
              children: <Widget>[
                Padding(
                    padding: const EdgeInsets.fromLTRB(5, 30, 5, 5),
                    child: SingleChildScrollView(child: ArrangementDataTable(agencyId: agency!.id))),
                Padding(
                    padding: const EdgeInsets.fromLTRB(5, 30, 5, 5),
                    child: SingleChildScrollView(child: ReservationDataTable(agencyId: agency!.id)))
              ],
            ),
          )
        ],
      ),
    );
  }
}
