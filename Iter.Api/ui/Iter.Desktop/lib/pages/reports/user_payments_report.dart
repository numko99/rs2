import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:ui/models/user_payment.dart';
import 'package:ui/services/report_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';

class UserPaymentReportPage extends StatefulWidget {
  final String arrangementId;
  final String arrangementName;
  final String? dateFrom;
  final String? dateTo;

  const UserPaymentReportPage({
    required this.arrangementId,
    required this.arrangementName,
    required this.dateFrom,
    required this.dateTo,
  });

  @override
  _UserPaymentReportPageState createState() => _UserPaymentReportPageState();
}

class _UserPaymentReportPageState extends State<UserPaymentReportPage> {
  List<UserPaymentModel> _userPayments = [];
  bool displayLoader = true;
  String? arrangementName;
  double totalSum = 0;
  ReportProvider? reportProvider;

  @override
  void initState() {
    super.initState();
    reportProvider = context.read<ReportProvider>();
    _loadData();
  }

  Future<void> _loadData() async {
    var _userPaymentsTemp = await reportProvider
        ?.getUserPayments({"arrangementId": widget.arrangementId!});

    if (_userPaymentsTemp == null) {
      return;
    }

    setState(() {
      _userPayments = _userPaymentsTemp;
      totalSum = _userPaymentsTemp.fold(
          0, (sum, payment) => sum + (payment.totalPaid ?? 0));
      arrangementName = _userPayments.length == 0 ? "" : _userPayments.first.arrangementName;
      displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Uplate korisnika'),
      ),
      body: displayLoader
          ? const Center(child: CircularProgressIndicator())
          : Padding(
            padding: const EdgeInsets.all(32.0),
            child: Card(
              child: Column(
                children: [
                  SizedBox(
                    height: 30,
                    child: Padding(
                      padding: const EdgeInsets.fromLTRB(15.0, 0, 0, 0),
                      child: Row(
                        children: [
                          Expanded(child:  Text(
                                'Aranžman: ${widget.arrangementName}',
                                style: const TextStyle(
                                    fontSize: 15, fontWeight: FontWeight.bold),
                              ),
                            ),
                             Expanded(
                              child: Text(
                                'Ukupno uplaćeno: ${totalSum}KM',
                                style: const TextStyle(
                                    fontSize: 15, fontWeight: FontWeight.bold),
                              ),
                            ),
                              Expanded(
                              child: Text(
                                'Broj uplata: ${_userPayments.length}',
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
                      children: [
                        Expanded(
                          child: _userPayments.length == 0 ? Center(child: Text("Nema uplata", style: TextStyle(fontSize: 20))) : SingleChildScrollView(
                            child: DataTable(
                              columns: [
                                const DataColumn(
                                    label: Text('Ime',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                                const DataColumn(
                                    label: Text('Prezime',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                                const DataColumn(
                                    label: Text('Broj transakcije',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                                const DataColumn(
                                    label: Text('Broj rezervacije',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                                const DataColumn(
                                    label: Text('Uplata',
                                        style:
                                            TextStyle(fontWeight: FontWeight.bold))),
                              ],
                              rows: _userPayments.map((payment) {
                                return DataRow(cells: [
                                  DataCell(Text(payment.firstName ?? '')),
                                  DataCell(Text(payment.lastName ?? '')),
                                  DataCell(Text(payment.transactionId ?? '')),
                                  DataCell(Text(payment.reservationNumber ?? '')),
                                  DataCell(Text(
                                      payment.totalPaid!.toString() + "KM" ?? '')),
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
