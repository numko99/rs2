import 'package:firedart/firestore/firestore.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:syncfusion_flutter_charts/charts.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/admin_statistc_response.dart';
import 'package:ui/models/reservation_diagram_response.dart';
import 'package:ui/services/statistic_provider.dart';
import 'package:ui/widgets/layout.dart';

class Home extends StatefulWidget {
  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  bool displayLoader = true;
  StatisticProvider? _statisticProvider;
  AdminStatisticResponse? adminStatisticResponse;
  int? totalMessages;

  @override
  void initState() {
    super.initState();

    _statisticProvider = context.read<StatisticProvider>();
    loadData();
  }

  Future<void> loadData() async {
    setState(() {
      displayLoader = true;
    });

    var tempData = await _statisticProvider?.getAvailableStatistics();
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

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Početna",
      icon: Icons.home,
      displayBackNavigationArrow: false,
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
                      icon: Icons.person,
                      title: "Korisnici",
                      subtitle: adminStatisticResponse!.usersCount.toString(),
                      color: Colors.blue,
                    ),
                    _buildDashboardCard(
                      icon: Icons.list_alt,
                      title: "Rezervacije",
                      subtitle:
                          adminStatisticResponse!.reservationCount.toString(),
                      color: Colors.green,
                    ),
                    _buildDashboardCard(
                      icon: Icons.business,
                      title: "Agencije",
                      subtitle:
                          adminStatisticResponse!.agenciesCount.toString(),
                      color: Colors.orange,
                    ),
                    _buildDashboardCard(
                      icon: Icons.beach_access_outlined,
                      title: "Aranžmani",
                      subtitle:
                          adminStatisticResponse!.arrangementCount.toString(),
                      color: Colors.red,
                    ),
                    _buildDashboardCard(
                      icon: Icons.message,
                      title: "Poruke",
                      subtitle: totalMessages.toString() ?? "0",
                      color: Colors.purple,
                    ),
                    _buildDashboardCard(
                      icon: Icons.monetization_on,
                      title: "Uplaćeno",
                      subtitle:
                          "${adminStatisticResponse!.totalAmount.toInt()} KM",
                      color: Colors.teal,
                    ),
                    Container(
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
                          title: AxisTitle(
                              text:
                                  'Broj rezervacija'),
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
