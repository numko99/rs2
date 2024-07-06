import 'package:flutter/material.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/reservation_search_respose.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:iter_mobile/widgets/reservation_card.dart';
import 'package:provider/provider.dart';

class MyArrangementsPage extends StatefulWidget {
  const MyArrangementsPage({super.key});

  @override
  State<MyArrangementsPage> createState() => _MyArrangementsPageState();
}

class _MyArrangementsPageState extends State<MyArrangementsPage>
    with SingleTickerProviderStateMixin {
  List<ReservationSearchResponse> reservations = [];
  TabController? _tabController;

  bool displayLoader = false;
  ReservationProvider? _reservationProvider;

  @override
  void initState() {
    super.initState();
    _reservationProvider = context.read<ReservationProvider>();
    _tabController = TabController(length: 2, vsync: this);
    _tabController!.addListener(_handleTabSelection);
    loadData(true);
  }

  @override
  void dispose() {
    _tabController?.dispose();
    super.dispose();
  }

  void _handleTabSelection() {
    if (_tabController!.indexIsChanging) {
      loadData(true);
    }
  }

  Future<void> loadData(bool refresh) async {
    try {
      if (!displayLoader) {
        setState(() {
          displayLoader = true;
        });
      }

      var filter = _tabController!.index == 0
          ? {'returnActiveReservations': true}
          : {'returnActiveReservations': false};
      var searchReservation = await _reservationProvider?.get(filter);

      if (searchReservation != null) {
        setState(() {
          displayLoader = false;
          reservations = searchReservation.result;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške: ${error.toString()}",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        toolbarHeight: 0,
        bottom: TabBar(
          labelColor: Colors.white,
          controller: _tabController,
          tabs: const [
            Tab(text: 'Aktivne Rezervacije'),
            Tab(text: 'Prethodna Putovanja'),
          ],
        ),
      ),
      body: displayLoader == true ?
      const Center(child: CircularProgressIndicator())
      : TabBarView(
        controller: _tabController,
        children: [
          reservations.isEmpty
              ? Center(child: Text('Nema rezervacija.'))
              : ListView.builder(
            itemCount: reservations.length,
            itemBuilder: (context, index) {
              return ReservationCard(reservation: reservations[index], onReturn: () => loadData(true),);
            },
          ),
          reservations.isEmpty
              ? Center(child: Text('Nema rezervacija.'))
              : ListView.builder(
            itemCount: reservations.length,
            itemBuilder: (context, index) {
              return ReservationCard(reservation: reservations[index], onReturn: () => {});
            },
          ),
        ],
      ),
    );
  }
}
