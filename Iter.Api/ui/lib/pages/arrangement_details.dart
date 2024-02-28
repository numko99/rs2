import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:ui/modals/arrangement_reservations_modal.dart';
import 'package:ui/models/arrangment.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/agency_details/arrangement_data_table.dart';
import 'package:ui/widgets/image_slider.dart';
import '../models/agency.dart';
import '../services/arrangment_provider.dart';

class ArrangementDetailsPage extends StatefulWidget {
  const ArrangementDetailsPage({super.key, required this.id});

  final String id;

  @override
  State<ArrangementDetailsPage> createState() => _ArrangementDetailsPageState();
}

class _ArrangementDetailsPageState extends State<ArrangementDetailsPage> {
  ArrangmentProvider? _arrangementProvider;
  ReservationProvider? _reservationProvider;

  Arrangement? arrangement;
  String? reservationCount;
  bool displayLoader = true;

  @override
  void initState() {
    super.initState();
    _arrangementProvider = context.read<ArrangmentProvider>();
    _reservationProvider = context.read<ReservationProvider>();

    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      displayLoader = true;
    });
    var reservationCountTemp = await _reservationProvider?.getCount(widget.id);
    var searchArrangement = await _arrangementProvider?.getById(widget.id);

    setState(() {
      reservationCount = reservationCountTemp.toString();
      arrangement = searchArrangement;
      displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Aranžman",
      icon: Icons.beach_access_outlined,
      body: displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : Row(
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
                  Expanded(
                    flex: 3,
                    child: Card(
                      child: Padding(
                        padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
                        child: Column(
                          children: [
                            Align(
                                alignment: Alignment.centerLeft,
                                child: Padding(
                                  padding:
                                      const EdgeInsets.fromLTRB(10.0, 10, 0, 5),
                                  child: Text(arrangement!.name,
                                      style: const TextStyle(fontSize: 20)),
                                )),
                            Align(
                                alignment: Alignment.centerLeft,
                                child: Padding(
                                  padding:
                                      const EdgeInsets.fromLTRB(10.0, 0, 0, 20),
                                  child: Text(
                                      "${DateFormat('dd.MM.yyyy.').format(arrangement!.startDate)}${arrangement!.endDate != null ? " - ${DateFormat('dd.MM.yyyy').format(arrangement!.endDate!)}" : ""}",
                                      style: const TextStyle(fontSize: 15)),
                                )),
                            ImageSlider(images: arrangement!.images),
                            Container(
                              margin: const EdgeInsets.symmetric(vertical: 8.0),
                              height: 1.0,
                              color: const Color.fromARGB(255, 227, 223, 223),
                            ),
                            const SizedBox(height: 10),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.start,
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Expanded(
                                  child: Align(
                                    alignment: Alignment.topCenter,
                                    child: Column(
                                      children: [
                                        const Icon(
                                          Icons.location_on,
                                          color: Colors.amber,
                                          size: 50.0,
                                        ),
                                        SizedBox(
                                        height: MediaQuery.of(context)
                                                .size
                                                .height *
                                            (arrangement!.destinations.length /
                                                7.5),
                                        child: ListView.builder(
                                          itemCount:
                                              arrangement!.destinations.length,
                                          itemBuilder: (context, index) {
                                            final destination = arrangement!
                                                .destinations[index];
                                            return Container(
                                              margin: const EdgeInsets.all(8.0),
                                              child: Column(
                                                crossAxisAlignment:
                                                    CrossAxisAlignment.start,
                                                children: [
                                                  Center(
                                                    child: Text(
                                                      '${destination.city}, ${destination.country}',
                                                      style: const TextStyle(
                                                        fontSize: 15.0,
                                                        color: Colors.amber,
                                                        fontWeight:
                                                            FontWeight.bold,
                                                      ),
                                                    ),
                                                  ),
                                                  const SizedBox(height: 8.0),
                                                  Center(
                                                    child: Text(
                                                        "${DateFormat('dd.MM.yyyy. hh:mm').format(destination.arrivalDate!)}${destination.accommodation != null ? " - ${DateFormat('dd.MM.yyyy').format(destination.departureDate!)}" : ""}"),
                                                  ),
                                                  if (destination
                                                          .accommodation !=
                                                      null) ...[
                                                    const SizedBox(
                                                        height: 8.0),
                                                    Center(
                                                      child: Text(
                                                        'Hotel: ${destination.accommodation?.hotelName ?? 'N/A'}',
                                                      ),
                                                    )
                                                  ]
                                                ],
                                              ),
                                            );
                                          },
                                        ),
                                      ),
                                      ],
                                    ),
                                  ),
                                ),
                                const Expanded(
                                  child: Expanded(
                                    child: Column(
                                      children: [
                                        Icon(
                                          Icons.assignment_rounded,
                                          color: Colors.amber,
                                          size: 50.0,
                                        ),
                                        Text(
                                          "Obilazak Ohrida, krstarenje Ohridskim jezerom – biserom Balkana, Makedonska noć, obilazak Skoplja -rodni grad Majke Terezije, posjeta zemlji orlova i gradu Tirani.  Program putovanja nije obavezan, ali je osmišljen kako bi omogućio 100% odmor, zabavu i nezaboravnih iskustava. Spakujte svoj kofer, mi ćemo se pobrinuti za ostalo!",
                                          style: TextStyle(
                                            fontSize: 14.0,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                ),
                                Expanded(
                                  child: Column(
                                    children: [
                                      const Icon(
                                        Icons.payment,
                                        color: Colors.amber,
                                        size: 50.0,
                                      ),
                                      ...arrangement!.prices
                                          .map((p) => Column(
                                                children: [
                                                  if (p.accommodationType != null)
                                                  Center(
                                                    child: Row(
                                                      mainAxisAlignment:
                                                          MainAxisAlignment
                                                              .center,
                                                      children: [
                                                        Text(
                                                          "${p.accommodationType} ",
                                                          style: const TextStyle(
                                                            fontSize: 14.0,
                                                          ),
                                                        ),
                                                        const Icon(Icons.hotel,
                                                            color: Colors.amber)
                                                      ],
                                                    ),
                                                  ),
                                                  Row(
                                                    mainAxisAlignment:
                                                        MainAxisAlignment.center,
                                                    children: [
                                                      const Text("Cijena: "),
                                                      Text("${p.price} KM",
                                                          style: const TextStyle(
                                                              color:
                                                                  Colors.amber)),
                                                    ],
                                                  ),
                                                  const SizedBox(height: 10)
                                                ],
                                              ))
                                          .toList()
                                    ],
                                  ),
                                )
                              ],
                            ),
                            Container(
                              margin: const EdgeInsets.symmetric(vertical: 8.0),
                              height: 1.0,
                              color: const Color.fromARGB(255, 227, 223, 223),
                            ),
                           const SizedBox(height: 10),
                            const Text("Detaljan opis", style: TextStyle(fontWeight: FontWeight.bold, fontSize: 20, color: Colors.amber)),
                            const SizedBox(height: 10),
                            Text(arrangement!.description),
                            const SizedBox(height: 100),

                        ],
                      ),
                    ),
                  ),
                ),
                Expanded(
                    child: Column(
                  children: [
                    Card(
                      child: Container(
                        padding: const EdgeInsets.all(8.0),
                        width: double.infinity,
                        child: const Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Padding(
                              padding: EdgeInsets.fromLTRB(16.0, 16, 8, 8),
                              child: Text("Vodiči: ",
                                  style: TextStyle(fontWeight: FontWeight.bold, fontSize: 15)),
                            ),
                            ListTile(
                              leading: Icon(Icons.person_outline,
                                  color: Colors.amber),
                              title: Text("Admir Numanović"),
                            ),
                            ListTile(
                              leading: Icon(Icons.person_outline,
                                  color: Colors.amber),
                              title: Text("Dalila Bajrić"),
                            ),
                          ],
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                    Container(
                      margin: const EdgeInsets.all(5.0),
                      width: double.infinity,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            "Broj rezervacija: $reservationCount",
                            style: const TextStyle(
                                fontSize: 16, fontWeight: FontWeight.bold),
                          ),
                          Container(
                            width: double.infinity,
                            height: 35,
                            margin: const EdgeInsets.only(top: 8.0),
                            child: ElevatedButton(
                              onPressed: () {
                                showDialog(
                                  context: context,
                                  builder: (BuildContext context) {
                                    return ArrangementReservationsModal(
                                        arrangementId: widget.id);
                                  },
                                );
                              },
                              child: const Text(
                                "Pogledaj rezervacije",
                                style: TextStyle(color: Colors.white),
                              ),
                            ),
                          ),
                        ],
                      ),
                    )
                  ],
                ))
              ],
            ),
    );
  }
}
