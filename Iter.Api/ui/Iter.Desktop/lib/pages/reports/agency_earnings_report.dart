import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/models/arrangement_earnings.dart';
import 'package:ui/models/user_payment.dart';
import 'package:ui/services/report_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';

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
        ?.geArrangementEarnings({"agencyId": widget.agencyId!});

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
        title: const Text('Uplate po aranžmanu'),
      ),
      body: displayLoader
          ? const Center(child: CircularProgressIndicator())
          : Padding(
            padding: const EdgeInsets.all(32.0),
            child: Card(
              child: Column(
                children: [
                  SizedBox(
                    height: 60,
                    child: Padding(
                      padding: const EdgeInsets.fromLTRB(15.0, 0, 0, 0),
                      child: Row(
                        children: [
                          Expanded(child:  Text(
                                'Agencija: ${widget.agencyName}',
                                style: const TextStyle(
                                    fontSize: 15, fontWeight: FontWeight.bold),
                              ),
                            ),
                             Expanded(
                              child: Text(
                                'Ukupna zarada: ${totalSum}KM',
                                style: const TextStyle(
                                    fontSize: 15, fontWeight: FontWeight.bold),
                              ),
                            ),
                              Expanded(
                              child: Text(
                                'Ukupan broj uplata: ${totalCount}',
                                style: const TextStyle(
                                    fontSize: 15, fontWeight: FontWeight.bold),
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
                          child: _userPayments.length == 0 ? Center(child: Text("Nema uplata", style: TextStyle(fontSize: 20))) : SingleChildScrollView(
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
