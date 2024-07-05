import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/employee_arrangement.dart';
import 'package:ui/services/employee_arrangment_provider.dart';

class ArrangementTouristGuideModal extends StatefulWidget {
  const ArrangementTouristGuideModal(
      {super.key, this.arrangementId, this.dateFrom, this.dateTo});

  final String? arrangementId;
  final DateTime? dateFrom;
  final DateTime? dateTo;
  @override
  ArrangementTouristGuideModalState createState() =>
      ArrangementTouristGuideModalState();
}

class ArrangementTouristGuideModalState
    extends State<ArrangementTouristGuideModal> {
  EmployeeArrangmentProvider? _employeeArrangementProvider;
  bool _displayLoader = true;
  List<DropdownModel> employeeeArangments = [];
  String? selectedEmployee;
  List<DropdownModel> availableEmployes = [];
  @override
  void initState() {
    super.initState();
    _employeeArrangementProvider = context.read<EmployeeArrangmentProvider>();

    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });

    var selectedEmployees = await _employeeArrangementProvider?.get({
      "arrangementId": widget.arrangementId,
    });
    availableEmployes =
        await _employeeArrangementProvider?.getAvailableEmployeeArrangments(
                widget.arrangementId, widget.dateFrom.toString(), widget.dateTo != null ? widget.dateTo.toString() : "") ??
            [];
    setState(() {
      availableEmployes = availableEmployes
          .where((element) =>
              !selectedEmployees!.result.any((x) => x.id == element.id))
          .toList();
      _displayLoader = false;
      employeeeArangments = selectedEmployees!.result
          .map((e) => e.employee!)
          .toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Text("Dodaj vodiča"),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: _displayLoader
          ? const Center(child: CircularProgressIndicator())
          : SizedBox(
              width: 600.0,
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  DropdownButtonFormField<dynamic>(
                      decoration: const InputDecoration(
                        labelText: 'Izaberite vodiča',
                      ),
                      value: selectedEmployee,
                      items: availableEmployes.map((DropdownModel item) {
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
                      onChanged: (value) async {
                        availableEmployes.forEach((x) => print(x.toJson()));
                        setState(() {
                          employeeeArangments.add(availableEmployes
                              .where((element) => element.id == value)
                              .first);
                          if (availableEmployes.length > 1) {
                            availableEmployes
                                .removeWhere((element) => element.id == value);
                            selectedEmployee = availableEmployes
                                .where((element) => element.id != value)
                                .firstOrNull
                                ?.id;
                            availableEmployes.forEach((x) => print(x.toJson()));
                            print(selectedEmployee);
                          } else {
                            availableEmployes = [];
                            selectedEmployee = null;
                          }
                        });
                      }),
                  SizedBox(height: 50),
                  Card(
                    child: SizedBox(
                      width: double.infinity,
                      child: SingleChildScrollView(
                        child: DataTable(
                          columns: const [
                            DataColumn(
                                label: Text('Ime i prezime',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(label: Text(""))
                          ],
                          rows: employeeeArangments
                              .map(
                                (employee) => DataRow(
                                  cells: [
                                    DataCell(Text(employee.name!)),
                                    DataCell(Align(
                                      alignment: Alignment.centerRight,
                                      child: IconButton(
                                          icon: const Icon(Icons.close),
                                          onPressed: () {
                                            setState(() {
                                              availableEmployes.add(
                                                  employeeeArangments
                                                      .where((element) =>
                                                          element.id ==
                                                          employee.id)
                                                      .first);
                                              selectedEmployee = null;
                                              employeeeArangments.removeWhere(
                                                  (element) =>
                                                      element.id ==
                                                      employee.id);
                                            });
                                          },
                                          tooltip: 'Obriši'),
                                    ))
                                  ],
                                ),
                              )
                              .toList(),
                        ),
                      ),
                    ),
                  ),
                  SizedBox(height: 50),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      ElevatedButton(
                        onPressed: () {
                          Navigator.of(context).pop();
                        },
                        style: const ButtonStyle(
                          backgroundColor:
                              MaterialStatePropertyAll<Color>(Colors.white70),
                        ),
                        child: const Text('Odustani'),
                      ),
                      const SizedBox(width: 8),
                      ElevatedButton(
                        onPressed: () async {
                          try{
                         await _employeeArrangementProvider?.insert({
                            "employeeIds":
                                employeeeArangments.map((e) => e.id).toList(),
                            "arrangementId": widget.arrangementId
                          });
                             ScaffoldMessengerHelper.showCustomSnackBar(
                                context: context,
                                message: "Vodiči uspješno dodani!",
                                backgroundColor: Colors.green);
                                Navigator.of(context).pop(true);
                          } catch (error) {
                            ScaffoldMessengerHelper.showCustomSnackBar(
                                context: context,
                                message: "Došlo je do greške",
                                backgroundColor: Colors.red);
                            Navigator.of(context).pop();
                          }
                        },
                        child: const Text(
                          'Sačuvaj',
                          style: TextStyle(color: Colors.white),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
    );
  }
}
