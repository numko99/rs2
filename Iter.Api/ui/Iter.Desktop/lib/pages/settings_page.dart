import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/widgets/layout.dart';
import 'package:ui/widgets/settings/city_data_table.dart';
import 'package:ui/widgets/settings/country_data_table.dart';

class SettingsPage extends StatefulWidget {
  const SettingsPage({super.key});

  @override
  State<SettingsPage> createState() => SettingsPageState();
}

class SettingsPageState extends State<SettingsPage> {
  DropdownProvider? _dropdownProvider;
  List<DropdownModel> countries = [];
  bool displayLoader = false;
  String _selectedOption = "Države";

  @override
  void initState() {
    super.initState();
    _dropdownProvider = context.read<DropdownProvider>();
    // initialLoad();
    // loadCities();
  }

  void _onOptionSelected(String option) {
    setState(() {
      _selectedOption = option;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      displayBackNavigationArrow: false,
      name: "Postavke",
      icon: Icons.settings,
      body: displayLoader
          ? const Center(
              child: CircularProgressIndicator(),
            )
          : Card(
            child: Padding(
              padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
              child: Container(
                child: Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Expanded(
                        flex: 1,
                        child: Column(
                          children: [
                            SizedBox(height: 30),
                            ListTile(
                              leading: const Icon(
                                Icons.flag_circle,
                                color: Colors.amber,
                                size: 25.0,
                              ),
                              title: const Text(
                                "Države",
                                style: TextStyle(
                                  fontSize: 20.0,
                                ),
                              ),
                              onTap: () => _onOptionSelected("Države"),
                              selected: _selectedOption == "Države",
                              selectedColor: Colors.amber,
                            ),
                            const SizedBox(height: 15),
                            ListTile(
                              leading: const Icon(
                                Icons.location_city_outlined,
                                color: Colors.amber,
                                size: 25.0,
                              ),
                              title: const Text(
                                "Gradovi",
                                style: TextStyle(
                                  fontSize: 20.0,
                                ),
                              ),
                              onTap: () => _onOptionSelected("Gradovi"),
                              selected: _selectedOption == "Gradovi",
                              selectedColor: Colors.amber,
                            ),
                          ],
                        ),
                      ),
                      Expanded(
                        flex: 5,
                        child: Container(
                          decoration: const BoxDecoration(
                            border: Border(
                              left: BorderSide(
                                color: Color.fromARGB(
                                    255, 241, 234, 234), // Boja ivice
                                width: 1.0, // Širina ivice
                              ),
                            ),
                          ),
                          child: Padding(
                            padding: const EdgeInsets.fromLTRB(32.0, 16, 32, 16),
                            child: _buildContent(),
                          ),
                        ),
                      ),
                    ],
                  ),
              ),
            ),
          ),
    );
  }

  Widget _buildContent() {
    switch (_selectedOption) {
      case "Države":
        return const CountryDataTable();
      case "Gradovi":
        return const CityDataTable();
      default:
        return const CountryDataTable();
    }
  }
}
