import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/models/arrangement_earnings.dart';
import 'package:ui/models/user_payment.dart';
import 'package:ui/services/report_provider.dart';
import 'package:ui/widgets/layout.dart';

class AgencyEarningsReportPage extends StatefulWidget {
  final String agencyId;
  final String agencyName;
  final String? dateFrom;
  final String? dateTo;

  const AgencyEarningsReportPage({
    required this.agencyId,
    required this.agencyName,
    required this.dateFrom,
    required this.dateTo,
  });

  @override
  AgencyEarningsReportPageState createState() => AgencyEarningsReportPageState();
}

class AgencyEarningsReportPageState extends State<AgencyEarningsReportPage> {
  List<ArrangementEarnings> _userPayments = [];
  bool displayLoader = true;
  String? arrangementName;
  double totalSum = 0;
  double totalCount = 0;
  ReportProvider? reportProvider;

  @override
  void initState() {
    super.initState();
    reportProvider = context.read<ReportProvider>();
    _loadData();
  }

  Future<void> _loadData() async {
    var _userPaymentsTemp = await reportProvider
        ?.geArrangementEarnings({"agencyId": widget.agencyId, "DateFrom": widget.dateFrom,
      "dateTo": widget.dateTo
    });

    if (_userPaymentsTemp == null) {
      return;
    }

    setState(() {
      _userPayments = _userPaymentsTemp;
      totalSum = _userPaymentsTemp.fold(
          0, (sum, payment) => sum + (payment.totalPaid ?? 0));
      totalCount = _userPaymentsTemp.fold(
          0, (sum, payment) => sum + (payment.reservationCount ?? 0));
      arrangementName = _userPayments.length == 0 ? "" : _userPayments.first.arrangementName;
      displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        iconTheme: const IconThemeData(
    color: Colors.white,
  ),
        title: const Text('Izvještaj o poslovanju', style: TextStyle(color: Colors.white)),
      ),
      body: displayLoader
          ? const Center(child: CircularProgressIndicator())
          : Padding(
            padding: const EdgeInsets.all(32.0),
            child: Card(
              child: Column(
                children: [
                    const Text("Uplate korisnika po aranžamu ",
                        style: TextStyle(fontSize: 20)),
                    const SizedBox(height: 30),
                  SizedBox(
                    height: 60,
                    child: Padding(
                      padding: const EdgeInsets.fromLTRB(15.0, 0, 0, 0),
                      child: Row(
                        children: [
                            Flexible(
                              flex: 2,
                              child: ListTile(
                                leading: const Text('Agencija:',
                                    style: TextStyle(
                                        fontSize: 15,
                                        fontWeight: FontWeight.bold)),
                                title: Text(widget.agencyName),
                              ),
                            ),
                            Flexible(
                              flex: 2,
                              child: ListTile(
                                leading: const Text('Ukupna zarada:',
                                    style: TextStyle(
                                        fontSize: 15,
                                        fontWeight: FontWeight.bold)),
                                title: Text("${totalSum}KM"),
                              ),
                            ),
                            Flexible(
                              flex: 1,
                              child: ListTile(
                                leading: const Text('Ukupan broj uplata:',
                                    style: TextStyle(
                                        fontSize: 15,
                                        fontWeight: FontWeight.bold)),
                                title: Text(totalCount.toInt().toString()),
                              ),
                            ),
                        ],
                      ),
                    ),
                  ),
                  Expanded(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      mainAxisAlignment: MainAxisAlignment.start,
                      children: [
                        Expanded(
                          child: _userPayments.length == 0 ? const Center(child: Text("Nema uplata", style: TextStyle(fontSize: 20))) : SingleChildScrollView(
                            child: DataTable(
                              columns: [
                                const DataColumn(
                                    label: Text('Aranžman',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                                const DataColumn(
                                    label: Text('Uplaćeno',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                                const DataColumn(
                                    label: Text('Broj uplata',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold)))
                              ],
                              rows: _userPayments.map((payment) {
                                return DataRow(cells: [
                                  DataCell(Text(payment.arrangementName ?? '')),
                                  DataCell(Text(payment.totalPaid.toString() + "KM" ?? '')),
                                  DataCell(Text(payment.reservationCount.toString() ?? ''))
                                ]);
                              }).toList(),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
    );
  }
}
