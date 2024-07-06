import 'dart:ffi';

import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/report_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';

class ReportsIndexPage extends StatefulWidget {
  const ReportsIndexPage({super.key});

  @override
  State<ReportsIndexPage> createState() => ReportsIndexPageState();
}

class ReportsIndexPageState extends State<ReportsIndexPage> {
  final _formKey = GlobalKey<FormBuilderState>();

  DropdownProvider? _dropdownProvider;
  ReportProvider? _reportProvider;

  bool displayLoader = true;

  int selectedReportType = 1;
  String? selectedArrangement;
  String? selectedAgency;
  List<DropdownModel>? agenciesDropdown;
  List<DropdownModel>? arrangementDropdown;

  @override
  void initState() {
    super.initState();
    _dropdownProvider = context.read<DropdownProvider>();
    _reportProvider = context.read<ReportProvider>();
    initialLoad();
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Izvještaji",
      icon: Icons.description,
      body: Card(
        child: FormBuilder(
          key: _formKey,
          child: Padding(
            padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
            child: Column(
              children: <Widget>[
                Row(
                  children: [
                    Expanded(
                      flex: 1,
                      child: DropdownButtonFormField<int>(
                        decoration: const InputDecoration(
                          labelText: 'Izaberite izvještaj',
                        ),
                        value: selectedReportType,
                        items: const [
                          DropdownMenuItem<int>(
                            value: 1,
                            child: Text('Izvještaji o uplatama korisnika'),
                          ),
                          DropdownMenuItem<int>(
                            value: 2,
                            child: Text('Izvještaj o poslovanju agencija'),
                          ),
                        ],
                        onChanged: (value) {
                          setState(() {
                            selectedReportType = value!;
                          });
                        },
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 30),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: <Widget>[
                    Expanded(
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.end,
                        children: <Widget>[
                          Expanded(
                            flex: 2,
                            child: DropdownButtonFormField<dynamic>(
                              decoration: const InputDecoration(
                                labelText: 'Izaberite agenciju',
                              ),
                              value: selectedAgency,
                              items:
                                  agenciesDropdown?.map((DropdownModel item) {
                                return DropdownMenuItem<dynamic>(
                                  value: item.id,
                                  child: Text(item.name ?? ""),
                                );
                              }).toList(),
                              validator: (value) {
                                if (value == null) {
                                  return 'Polje je obavezno';
                                }
                                return null;
                              },
                              onChanged: (value) {
                                test(value);
                              },
                            ),
                          ),
                          if (selectedReportType == 1) ...[
                            const SizedBox(width: 20),
                            Expanded(
                              flex: 2,
                              child: DropdownButtonFormField<String?>(
                                  decoration: const InputDecoration(
                                    labelText: 'Izaberite aranžman',
                                  ),
                                  value: selectedArrangement,
                                  items: arrangementDropdown
                                      ?.map((DropdownModel item) {
                                    return DropdownMenuItem<String>(
                                      value: item.id,
                                      child: Text(item.name ?? ""),
                                    );
                                  }).toList(),
                                  validator: (value) {
                                    if (value == null) {
                                      return 'Polje je obavezno';
                                    }
                                    return null;
                                  },
                                  onChanged: (String? value) {
                                    setState(() {
                                      selectedArrangement = value;
                                    });
                                  }),
                            ),
                          ],
                          const SizedBox(width: 20),
                          Expanded(
                            flex: 1,
                            child: ElevatedButton(
                                onPressed: generateReport,
                                child: const Text("Generiši izvještaj",
                                    style: TextStyle(color: Colors.white))),
                          ),
                          const Spacer(),
                          const Spacer(),
                        ],
                      ),
                    )
                  ],
                ),
                const SizedBox(height: 30)
              ],
            ),
          ),
        ),
      ),
    );
  }

  void generateReport() {
    if (_formKey.currentState?.saveAndValidate() ?? false) {
      if (selectedReportType == 1) {
       Navigator.pushNamed(context, '/report/userPayments', arguments: {
          'id': selectedArrangement,
          'name': arrangementDropdown?.where((element) => element.id == selectedArrangement).first.name,
          'dateFrom': null,
          'dateTo': null,
        });
      }
      if (selectedReportType == 2) {
         Navigator.pushNamed(context, '/report/arrangementEarnings', arguments: {
          'id': selectedAgency,
          'name': agenciesDropdown
              ?.where((element) => element.id == selectedAgency)
              .first
              .name,
          'dateFrom': null,
          'dateTo': null,
        });
      }
    }
  }

  Future<void> test(value) async {
    setState(() {
      selectedAgency = value;
    });
    var arrangements = await _dropdownProvider!.get(
        {"dropdownType": DropdownTypes.arrangements.index, "agencyId": value});
    setState(() {
      arrangementDropdown = arrangements.result;
    });
  }

  Future<void> initialLoad() async {
    try {
      var agenciesDropdownTemp = await _dropdownProvider!
          .get({"dropdownType": DropdownTypes.agencies});

      if (agenciesDropdownTemp.result.isNotEmpty) {
        setState(() {
          agenciesDropdown = agenciesDropdownTemp.result;
          displayLoader = false;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    }
  }
}
