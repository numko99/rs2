import 'package:firedart/firestore/firestore.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:syncfusion_flutter_charts/charts.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/custom_confirmation_modal.dart';
import 'package:ui/modals/details_reservation_modal.dart';
import 'package:ui/modals/update_reservation_modal.dart';
import 'package:ui/models/admin_statistc_response.dart';
import 'package:ui/models/reservation_diagram_response.dart';
import 'package:ui/models/reservation_search_respose.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/services/statistic_provider.dart';
import 'package:ui/widgets/layout.dart';
import 'package:ui/widgets/reservation_mini_data_table.dart';
import 'package:ui/widgets/reservation_status.dart';

class EmployeeHome extends StatefulWidget {
  @override
  State<EmployeeHome> createState() => _EmployeeHomeState();
}

class _EmployeeHomeState extends State<EmployeeHome> {
  bool displayLoader = true;
  StatisticProvider? _statisticProvider;
  ReservationProvider? _reservationProvider;
  AdminStatisticResponse? adminStatisticResponse;
  List<ReservationSearchResponse> reservations = [];
  int? totalMessages;
  String? agencyId;
  @override
  void initState() {
    super.initState();

    _statisticProvider = context.read<StatisticProvider>();
    _reservationProvider = context.read<ReservationProvider>();
    loadData();
  }

  Future<void> loadData() async {
    setState(() {
      displayLoader = true;
    });

    agencyId = AuthStorageProvider.getAuthData()?["agencyId"];
    search();
    var tempData = await _statisticProvider?.getEmployeeStatistics(agencyId);
    var chatCollection = Firestore.instance.collection('chats');
    var messageCount = 0;
    try{
      var documents = await chatCollection.get();
      messageCount = documents.length;
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Poruke trenutno nisu dostupne",
          backgroundColor: Colors.red);
    }

    setState(() {
      totalMessages = messageCount;
      adminStatisticResponse = tempData;
      displayLoader = false;
    });
  }

   Future<void> search() async {
    var searchReservation = await _reservationProvider?.get({
      "currentPage": 1,
      "pageSize": 5,
      "agencyId": agencyId,
      "reservationStatusId": 1,
    });

    if (searchReservation != null) {
      setState(() {
        reservations = searchReservation.result;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Početna",
      icon: Icons.home,
      body: displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : Card(
              child: Padding(
                padding: const EdgeInsets.all(16.0),
                child: Wrap(
                  spacing: 16.0,
                  runSpacing: 16.0,
                  children: [
                    _buildDashboardCard(
                      icon: Icons.list_alt,
                      title: "Rezervacije",
                      subtitle:
                          adminStatisticResponse!.reservationCount.toString(),
                      color: Colors.green,
                    ),
                    _buildDashboardCard(
                      icon: Icons.beach_access_outlined,
                      title: "Aranžmani",
                      subtitle:
                          adminStatisticResponse!.arrangementCount.toString(),
                      color: Colors.red,
                    ),
                    _buildDashboardCard(
                      icon: Icons.monetization_on,
                      title: "Uplaćeno",
                      subtitle:
                          "${adminStatisticResponse!.totalAmount.toInt()} KM",
                      color: Colors.teal,
                    ),
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Expanded(
                          flex: 2,
                          child: Column(
                            children: [
                              SizedBox(height: 25),
                              Text("Najnovije rezervacije", style: TextStyle(fontSize: 20)),
                              SizedBox(height: 15),
                              Container(
                                width: double.infinity,
                                child: DataTable(
                                  columns: const [
                                      DataColumn(
                                          label: Text('Broj rezervacije',
                                              style: TextStyle(
                                                  fontWeight: FontWeight.bold))),
                                      DataColumn(
                                          label: Text('Ime i prezime',
                                              style: TextStyle(
                                                  fontWeight: FontWeight.bold))),
                                    DataColumn(
                                        label: Text('Putovanje',
                                            style: TextStyle(
                                                fontWeight: FontWeight.bold))),
                                    DataColumn(
                                        label: Text('Status',
                                            style: TextStyle(
                                                fontWeight: FontWeight.bold))),
                                    DataColumn(label: Text('')),
                                  ],
                                  rows: reservations
                                      .map(
                                        (reservation) => DataRow(
                                          cells: [
                                              DataCell(
                                                  Text(reservation.reservationNumber)),
                                              DataCell(Text(
                                                  "${reservation.firstName} ${reservation.lastName}")),
                                            DataCell(Text(reservation.arrangementName)),
                                            DataCell(ReservationStatus(
                                                statusId:
                                                    reservation.reservationStatusId,
                                                status:
                                                    reservation.reservationStatusName)),
                                            DataCell(
                                              SizedBox(
                                                width: double.infinity,
                                                child: Row(
                                                  mainAxisAlignment:
                                                      MainAxisAlignment.end,
                                                  children: <Widget>[
                                                    IconButton(
                                                        icon: const Icon(
                                                            Icons.open_in_new_off),
                                                        onPressed: () {
                                                          showDialog(
                                                            context: context,
                                                            builder:
                                                                (BuildContext context) {
                                                              return ReservationDetailsModal(
                                                                  reservationId:
                                                                      reservation
                                                                          .reservationId);
                                                            },
                                                          );
                                                        },
                                                        tooltip: 'Detalji'),
                                                    IconButton(
                                                        icon: const Icon(
                                                            Icons.edit_document),
                                                        onPressed: () {
                                                          showDialog(
                                                            context: context,
                                                            builder:
                                                                (BuildContext context) {
                                                              return UpdateReservationModal(
                                                                  onCompleted: () =>
                                                                      search(),
                                                                  reservationId:
                                                                      reservation
                                                                          .reservationId);
                                                            },
                                                          );
                                                        },
                                                        tooltip: 'Uredi')
                                                  ],
                                                ),
                                              ),
                                            ),
                                          ],
                                        ),
                                      )
                                      .toList(),
                                ),
                              ),
                            ],
                          ),
                        ),
                        Expanded(
                          child: Container(
                            padding: const EdgeInsets.all(16),
                            child: SfCartesianChart(
                                  title: ChartTitle(text: 'Broj rezervacija po mjesecima'), 
                          
                              primaryXAxis: DateTimeAxis(
                                dateFormat:
                                    DateFormat.yMMM(),
                                intervalType: DateTimeIntervalType
                                    .months,
                              ),
                              primaryYAxis: NumericAxis(
                              ),
                              series: <LineSeries<ReservationDiagramResponse,
                                  DateTime>>[
                                LineSeries<ReservationDiagramResponse, DateTime>(
                                  dataSource: adminStatisticResponse!.reservations,
                                  xValueMapper:
                                      (ReservationDiagramResponse data, _) =>
                                          data.date,
                                  yValueMapper:
                                      (ReservationDiagramResponse data, _) =>
                                          data.reservationCount,
                                )
                              ],
                            ),
                          ),
                        ),
                      ],
                    )
                  ],
                ),
              ),
            ),
    );
  }

  Widget _buildDashboardCard({
    required IconData icon,
    required String title,
    required String subtitle,
    required Color color,
  }) {
    return SizedBox(
      width: (MediaQuery.of(context).size.width / 3) -
          32, // Adjust card width to fit 2 per row
      child: Card(
        elevation: 4,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Icon(
                icon,
                size: 40,
                color: color,
              ),
              SizedBox(height: 10),
              Text(
                title,
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                  color: color,
                ),
              ),
              SizedBox(height: 5),
              Text(
                subtitle,
                style: TextStyle(
                  fontSize: 16,
                  color: Colors.grey[700],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
