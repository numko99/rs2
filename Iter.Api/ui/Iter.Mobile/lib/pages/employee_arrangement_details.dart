import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/enums/reservation_status.dart';
import 'package:iter_mobile/helpers/date_time_helper.dart';
import 'package:iter_mobile/modals/reservation_details_modal.dart';
import 'package:iter_mobile/models/reservation_search_respose.dart';
import 'package:iter_mobile/pages/chat_page.dart';
import 'package:iter_mobile/providers/auth_provider.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:iter_mobile/widgets/destination_by_country_card.dart';
import 'package:iter_mobile/widgets/icon_text_chip.dart';
import 'package:iter_mobile/models/arrangment.dart';
import 'package:iter_mobile/providers/arrangment_provider.dart';
import 'package:provider/provider.dart';

class EmployeeArrangementDetailsPage extends StatefulWidget {
  const EmployeeArrangementDetailsPage({super.key, required this.id});

  final String id;

  @override
  State<EmployeeArrangementDetailsPage> createState() =>
      _EmployeeArrangementDetailsPageState();
}

class _EmployeeArrangementDetailsPageState
    extends State<EmployeeArrangementDetailsPage> {
  ArrangmentProvider? _arrangementProvider;
  ReservationProvider? _reservationProvider;

  Arrangement? arrangement;
  List<ReservationSearchResponse>? reservations;
  final TextEditingController searchController = TextEditingController();

  bool displayLoader = true;

  @override
  void initState() {
    super.initState();
    _arrangementProvider = context.read<ArrangmentProvider>();
    _reservationProvider = context.read<ReservationProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    if (mounted) {
      setState(() {
        displayLoader = true;
      });
    }
    var searchArrangement = await _arrangementProvider?.getById(widget.id);
    await loadReservations();
    if (mounted) {
      setState(() {
        arrangement = searchArrangement;
        displayLoader = false;
      });
    }
  }

  Future<void> loadReservations() async {
    var searchReservations = await _reservationProvider?.get({
      "name": searchController.text,
      "arrangementId": widget.id,
      "reservationStatusId":
          (ReservationStatusEnum.confirmed.index + 1).toString(),
    });
    if (mounted) {
      setState(() {
        reservations = searchReservations?.result;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey[300],
      appBar: AppBar(),
      body: displayLoader
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Card(
                    child: SizedBox(
                      width: MediaQuery.of(context).size.width,
                      child: Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(arrangement!.name,
                                style: TextStyle(
                                  fontSize: 20.0,
                                  color: Colors.grey[600],
                                  fontWeight: FontWeight.bold,
                                )),
                            const SizedBox(height: 20),
                            Wrap(spacing: 8.0, runSpacing: 4.0, children: [
                              IconTextChip(
                                  iconData: Icons.calendar_today,
                                  backgroundColor: Colors.grey[400]!,
                                  label:
                                      "${DateFormat('dd.MM.yyyy.').format(arrangement!.startDate)}${arrangement!.endDate != null ? " - ${DateFormat('dd.MM.yyyy').format(arrangement!.endDate!)}" : ""}"),
                              IconTextChip(
                                  iconData: Icons.watch_later_outlined,
                                  backgroundColor: Colors.grey[400]!,
                                  label:
                                      "Prije ${DateTimeCustomHelper.getTimeSince(arrangement!.modifiedAt!)}"),
                              if (arrangement!.endDate != null
                                  ? DateTimeCustomHelper.isAfterOrEqualDateOnly(
                                          arrangement!.endDate!,
                                          DateTime.now()) &&
                                      DateTimeCustomHelper
                                          .isBeforeOrEqualDateOnly(
                                              arrangement!.startDate,
                                              DateTime.now())
                                  : DateTimeCustomHelper.areDatesEqual(
                                      arrangement!.startDate,
                                      DateTime.now())) ...[
                                const Chip(
                                    label: Text("Putovanje u toku",
                                        style: TextStyle(color: Colors.white)),
                                    backgroundColor: Colors.green),
                              ] else if (DateTimeCustomHelper.isBeforeDateOnly(
                                  arrangement!.startDate, DateTime.now()))
                                const Chip(
                                    label: Text("Završeno",
                                        style: TextStyle(color: Colors.white)),
                                    backgroundColor: Colors.red),
                            ]),
                            const SizedBox(height: 20),
                            Divider(
                              height: 10,
                              color: Colors.grey[300],
                              thickness: 1, // Debljina linije
                            ),
                            DestinationsByCountryCard(
                                destinations: arrangement!.destinations),
                            const SizedBox(height: 10),
                          ],
                        ),
                      ),
                    ),
                  ),
                  Card(
                    child: Column(
                      children: [
                        Align(
                            alignment: Alignment.centerLeft,
                            child: Padding(
                              padding:
                                  const EdgeInsets.fromLTRB(16.0, 8, 16, 8),
                              child: Text("Spisak putnika",
                                  style: TextStyle(
                                    fontSize: 20.0,
                                    color: Colors.grey[600],
                                    fontWeight: FontWeight.bold,
                                  )),
                            )),
                        SizedBox(
                          height: 45,
                          child: Padding(
                            padding:
                                const EdgeInsets.fromLTRB(11.0, 8.0, 11.0, 0),
                            child: TextFormField(
                              controller: searchController,
                              onChanged: (String value) async {
                                if (value.length > 3 || value.length == 0) {
                                  await loadReservations();
                                }
                              },
                              decoration: InputDecoration(
                                  contentPadding: const EdgeInsets.all(16),
                                  labelText: 'Pretraga putnika...',
                                  border: const OutlineInputBorder(),
                                  suffixIcon: IconButton(
                                      icon: const Icon(Icons.search),
                                      onPressed: () async =>
                                          await loadReservations())),
                            ),
                          ),
                        ),
                        SingleChildScrollView(
                          child: Row(children: <Widget>[
                            Expanded(
                              child: DataTable(
                                columns: const [
                                  DataColumn(
                                      label: Text('Ime i prezime',
                                          style: TextStyle(
                                              fontWeight: FontWeight.bold))),
                                  DataColumn(label: Text("")),
                                ],
                                rows: reservations!
                                    .asMap()
                                    .entries
                                    .map((reservation) => DataRow(
                                          cells: [
                                            DataCell(Text(
                                                "${reservation.value.firstName} ${reservation.value.lastName}")),
                                            DataCell(Align(
                                              alignment: Alignment.centerRight,
                                              child: Row(
                                                mainAxisAlignment:
                                                    MainAxisAlignment.end,
                                                children: [
                                                  IconButton(
                                                      icon: const Icon(Icons
                                                          .open_in_new_off),
                                                      onPressed: () => {
                                                            showDialog(
                                                              context: context,
                                                              builder:
                                                                  (BuildContext
                                                                      context) {
                                                                return ReservationDetailsModal(
                                                                    reservationId:
                                                                        reservation
                                                                            .value
                                                                            .reservationId);
                                                              },
                                                            )
                                                          }),
                                                  IconButton(
                                                      icon: const Icon(
                                                          Icons.message),
                                                      onPressed: () => {
                                                            Navigator.of(context).push(
                                                                MaterialPageRoute(
                                                                    builder:
                                                                        (context) =>
                                                                            ChatPage(
                                                                              senderId: reservation.value.reservationId,
                                                                              receiverId: AuthStorageProvider.getAuthData()?["id"],
                                                                              reciverName: "${reservation.value.firstName} ${reservation.value.lastName}",
                                                                              reciverAgency: "",
                                                                            )))
                                                          }),
                                                ],
                                              ),
                                            )),
                                          ],
                                        ))
                                    .toList(),
                              ),
                            ),
                          ]),
                        ),
                      ],
                    ),
                  )
                ],
              ),
            ),
    );
  }
}
