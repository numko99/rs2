import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/helpers/date_time_helper.dart';
import 'package:iter_mobile/widgets/agency_card.dart';
import 'package:iter_mobile/widgets/destination_by_country_card.dart';
import 'package:iter_mobile/widgets/expandable_text.dart';
import 'package:iter_mobile/widgets/icon_text_chip.dart';
import 'package:iter_mobile/widgets/image_slider.dart';
import 'package:iter_mobile/models/arrangment.dart';
import 'package:iter_mobile/providers/arrangment_provider.dart';
import 'package:iter_mobile/widgets/prices_card.dart';
import 'package:iter_mobile/widgets/reservation_confirmation_dialog.dart';
import 'package:provider/provider.dart';

class ArrangementDetailsPage extends StatefulWidget {
  const ArrangementDetailsPage({super.key, required this.id, this.isReserved});

  final String id;
  final bool? isReserved;

  @override
  State<ArrangementDetailsPage> createState() => _ArrangementDetailsPageState();
}

class _ArrangementDetailsPageState extends State<ArrangementDetailsPage> {
  ArrangmentProvider? _arrangementProvider;
  Arrangement? arrangement;
  bool displayLoader = true;

  @override
  void initState() {
    super.initState();
    _arrangementProvider = context.read<ArrangmentProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      displayLoader = true;
    });
    var searchArrangement = await _arrangementProvider?.getById(widget.id);
    setState(() {
      arrangement = searchArrangement;
      displayLoader = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey[300],
      body: displayLoader
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Stack(
                    children: [
                      ImageSlider(images: arrangement!.images),
                      Positioned(
                        top: MediaQuery.of(context).padding.top,
                        left: 0,
                        child: SafeArea(
                          child: IconButton(
                            icon: Icon(Icons.arrow_back,
                                color: Theme.of(context).primaryColor),
                            onPressed: () => Navigator.of(context).pop(),
                          ),
                        ),
                      ),
                    ],
                  ),
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
                            if (!(arrangement!.prices.length == 1 &&
                                arrangement!.prices[0].accommodationType ==
                                    null))
                              Divider(
                                height: 15,
                                color: Colors.grey[300],
                                thickness: 1, // Debljina linije
                              ),
                            ...arrangement!.prices
                                .map((p) => PriceDetail(price: p))
                                .toList(),
                            const SizedBox(height: 15),
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
                                      "Prije ${DateTimeCustomHelper.getTimeSince(arrangement!.modifiedAt!)}")
                            ]),
                            const SizedBox(height: 15),
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
                  AgencyCard(agency: arrangement!.agency),
                  Card(
                      child: Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Padding(
                          padding: const EdgeInsets.only(left: 8.0),
                          child: Text("DETALJAN OPIS",
                              style: TextStyle(
                                  fontSize: 12,
                                  fontWeight: FontWeight.bold,
                                  color: Colors.grey[600])),
                        ),
                        SizedBox(
                          width: double.infinity,
                          child: Card(
                              color: Colors.grey[100],
                              child: Padding(
                                padding: const EdgeInsets.all(8.0),
                                child: ExpandableText(
                                    text: arrangement!.description),
                              )),
                        ),
                      ],
                    ),
                  )),
                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: ElevatedButton(
                        onPressed: () => widget.isReserved == true
                         ? () => {}
                         : showModalBottomSheet(
                            context: context,
                            builder: (BuildContext bc) {
                              return ReservationConfirmationDialog(
                                  arrangementId: arrangement!.id);
                            }),
                         style: ElevatedButton.styleFrom(
                          minimumSize: const Size(double.infinity, 50),
                          backgroundColor: widget.isReserved == true
                              ? Colors.grey
                              : Colors.amber,
                          foregroundColor: widget.isReserved == true
                              ? Colors.black
                              : Colors.white,
                          disabledBackgroundColor: Colors.grey,
                        ),
                        child: const Text('Rezerviši sada',
                            style: TextStyle(color: Colors.white)),
                      ),
                    ),
                  )
                ],
              ),
            ),
    );
  }
}
