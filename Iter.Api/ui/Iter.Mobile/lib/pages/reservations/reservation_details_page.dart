import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_stripe/flutter_stripe.dart' as stripe;
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/enums/reservation_status.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/employee_arrangement.dart';
import 'package:iter_mobile/models/reservation.dart';
import 'package:iter_mobile/pages/chat/chat_page.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';
import 'package:iter_mobile/providers/employee_arrangment_provider.dart';
import 'package:iter_mobile/providers/reservation_provider.dart';
import 'package:provider/provider.dart';

class ReservationDetailsScreen extends StatefulWidget {
  const ReservationDetailsScreen({super.key, this.reservationId});

  final String? reservationId;

  @override
  _ReservationDetailsScreenState createState() =>
      _ReservationDetailsScreenState();
}

class _ReservationDetailsScreenState extends State<ReservationDetailsScreen> {
  bool displayLoader = true;
  Reservation? reservation;
  List<EmployeeArrangment> employees = [];
  ReservationProvider? reservationProvider;
  EmployeeArrangmentProvider? _employeeArrangmentProvider;

  @override
  void initState() {
    super.initState();
    reservationProvider = context.read<ReservationProvider>();
    _employeeArrangmentProvider = context.read<EmployeeArrangmentProvider>();

    loadData();
  }

  Future<void> loadData() async {
    var reservationTemp =
        await reservationProvider!.getById(widget.reservationId);

    var employeesTemp = await _employeeArrangmentProvider?.get({
      "arrangementId": reservationTemp.arrangement?.id,
    });

    setState(() {
      reservation = reservationTemp;
      employees = employeesTemp!.result;
      displayLoader = false;
    });
  }

  showPaymentSheet() async {
    setState(() {
      displayLoader = true;
    });
    try {
      var paymentIntentData = await createPaymentIntent(
          ((reservation!.arrangementPrice!.price)! * 100).round().toString(),
          "BAM");
      await reservationProvider!.addPayment(reservation!.id,
          reservation!.arrangementPrice?.price, paymentIntentData['id']);
      await stripe.Stripe.instance
          .initPaymentSheet(
        paymentSheetParameters: stripe.SetupPaymentSheetParameters(
          paymentIntentClientSecret: paymentIntentData['client_secret'],
          merchantDisplayName: 'ITer',
          appearance: const stripe.PaymentSheetAppearance(
            primaryButton: stripe.PaymentSheetPrimaryButtonAppearance(
                colors: stripe.PaymentSheetPrimaryButtonTheme(
                    light: stripe.PaymentSheetPrimaryButtonThemeColors(
                        background: Colors.cyan))),
          ),
        ),
      )
          .then((value) {
        print(value);
      }).onError((error, stackTrace) {
        print(error);
        showDialog(
            context: context,
            builder: (_) => AlertDialog(
                  content: Text("Došlo je do greške"),
                ));
      });

      await stripe.Stripe.instance.presentPaymentSheet();
      await reservationProvider!.addPayment(reservation!.id,
          reservation!.arrangementPrice?.price, paymentIntentData['id']);
      await loadData();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste izvršili upratu",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške: ${error.toString()}",
          backgroundColor: Colors.red);
    }
    setState(() {
      displayLoader = false;
    });
  }

  createPaymentIntent(String amount, String currency) async {
    try {
      Map<String, dynamic> body = {
        'amount': amount,
        'currency': currency,
        'payment_method_types[]': 'card'
      };
      var response = await http.post(
          Uri.parse('https://api.stripe.com/v1/payment_intents'),
          body: body,
          headers: {
            'Authorization':
                'Bearer sk_test_51PQwmlBosSiX3Jj5LSDOx1OhPtIEvx5nVNYOUvnHxFPF1pRskUols4f51eNGzYV1HCNKlQcLGdSYoMK343iOOeB3007ucSXi2z',
            'Content-Type': 'application/x-www-form-urlencoded'
          });
      return jsonDecode(response.body);
    } catch (err) {
      //silent
    }
  }

  @override
  Widget build(BuildContext context) {
    return displayLoader == true
        ? const Center(child: CircularProgressIndicator())
        : Scaffold(
            body: SafeArea(
              child: SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    if (reservation?.arrangement!.images.isNotEmpty ?? false)
                      Stack(
                        children: [
                          Image.memory(
                            base64Decode(reservation!.arrangement!.images
                                .firstWhere((image) => image.isMainImage == true)
                                .image),
                            width: double.infinity,
                            height: 150,
                            fit: BoxFit.cover,
                          ),
                          Positioned(
                            top: 1,
                            left: 0,
                            child: SafeArea(
                              child: IconButton(
                                  icon: Icon(Icons.arrow_back,
                                      color: Theme.of(context).primaryColor),
                                  onPressed: () {
                                    if (mounted) {
                                      Navigator.of(context).pop();
                                    }
                                  }),
                            ),
                          ),
                        ],
                      ),
                    Card(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Card(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(reservation!.arrangement!.name,
                                    style: TextStyle(
                                      fontSize: 20.0,
                                      color: Colors.grey[600],
                                      fontWeight: FontWeight.bold,
                                    )),
                                Text(reservation!.arrangement!.agency.name,
                                    style: TextStyle(
                                      fontSize: 14.0,
                                      color: Colors.grey[500],
                                      fontWeight: FontWeight.bold,
                                    )),
                                const SizedBox(height: 15),
                                ListTile(
                                    leading: const Icon(Icons.calendar_month),
                                    title: Text(
                                        "${DateFormat('dd.MM.yyyy.').format(reservation!.arrangement!.startDate)}${reservation!.arrangement!.endDate != null ? " - ${DateFormat('dd.MM.yyyy').format(reservation!.arrangement!.endDate!)}" : ""}")),
                              ],
                            ),
                          ),
                          ListTile(
                            leading: Icon(Icons.check_circle,
                                color:
                                    getColor(reservation!.reservationStatusId!)),
                            title: Text(reservation!.reservationStatusName!),
                            subtitle: const Text("Status rezervacije"),
                          ),
                          ListTile(
                            leading: const Icon(Icons.place),
                            title: Text(reservation!.departurePlace ?? "-"),
                            subtitle: const Text("Mjesto polaska"),
                          ),
                          if (reservation!.arrangementPrice != null &&
                              reservation!.arrangementPrice!.accommodationType !=
                                  null)
                            ListTile(
                              leading: const Icon(Icons.hotel),
                              title: Text(reservation!
                                  .arrangementPrice!.accommodationType!),
                              subtitle: const Text("Tip smještaja"),
                            ),
                          ListTile(
                            leading: const Icon(Icons.monetization_on),
                            title: Text(
                                "${reservation!.totalPaid!.toInt()}/${reservation!.arrangementPrice?.price?.toInt() ?? 0} KM"),
                            subtitle: const Text("Status uplate"),
                          ),
                          Row(children: <Widget>[
                            if (employees.isNotEmpty)
                            Expanded(
                              child: DataTable(
                                columns: const [
                                  DataColumn(
                                      label: Text('Vodiči',
                                          style: TextStyle(
                                              fontWeight: FontWeight.bold))),
                                  DataColumn(label: Text("")),
                                ],
                                rows: employees
                                    .asMap()
                                    .entries
                                    .map((employee) => DataRow(
                                          cells: [
                                            DataCell(Text(
                                                "${employee.value.employee!.firstName!} ${employee.value.employee!.lastName!}")),
                                            DataCell(Align(
                                              alignment: Alignment.centerRight,
                                              child: Row(
                                                mainAxisAlignment:
                                                    MainAxisAlignment.end,
                                                children: [
                                                  IconButton(
                                                      icon: const Icon(
                                                          Icons.message),
                                                      onPressed: () => {
                                                            Navigator.of(context).push(
                                                                MaterialPageRoute(
                                                                    builder:
                                                                        (context) =>
                                                                            ChatPage(
                                                                              receiverId:
                                                                                  employee.value.employee!.userId!,
                                                                              senderId:
                                                                                  AuthStorageProvider.getAuthData()?["id"],
                                                                              reciverName:
                                                                                  "${employee.value.employee!.firstName!} ${employee.value.employee!.lastName!}",
                                                                              reciverAgency:
                                                                                  employee.value.arrangement?.agencyName ?? "",
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
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
            bottomNavigationBar: getBottomNavigationBar());
  }

  Widget? getBottomNavigationBar() {
    if (reservation!.reservationStatusId ==
        ReservationStatusEnum.pending.index + 1) {
      return BottomAppBar(
        padding: EdgeInsets.all(8.0),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            ElevatedButton(
              onPressed: () async {
                await showPaymentSheet();
              },
              child: const Text('Izvrši plaćanje',
                  style: TextStyle(color: Colors.white)),
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(backgroundColor: Colors.red[400]),
              onPressed: () async {
                final bool? confirm = await showDialog<bool>(
                  context: context,
                  builder: (BuildContext context) {
                    return AlertDialog(
                      title: const Text('Potvrda'),
                      content: const Text(
                          'Da li ste sigurni da želite otkazati rezervaciju?'),
                      actions: <Widget>[
                        TextButton(
                          onPressed: () {
                            if (mounted) {
                              Navigator.of(context).pop(false);
                            }
                          },
                          child: const Text('Ne'),
                        ),
                        TextButton(
                          onPressed: () {
                            if (mounted) {
                              Navigator.of(context).pop(true);
                            }
                          },
                          child: const Text('Da'),
                        ),
                      ],
                    );
                  },
                );

                if (confirm == true) {
                  setState(() {
                    displayLoader = true;
                  });
                  try {
                    await reservationProvider!
                        .cancelReservation(reservation!.id);
                    await loadData();
                    ScaffoldMessengerHelper.showCustomSnackBar(
                        context: context,
                        message: "Uspješno ste otkazali rezervaciju",
                        backgroundColor: Colors.green);
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
              },
              child: const Text('Otkaži rezervaciju',
                  style: TextStyle(color: Colors.white)),
            ),
          ],
        ),
      );
    } else if ((reservation!.reservationStatusId ==
            ReservationStatusEnum.confirmed.index + 1) &&
        ((reservation!.arrangement!.endDate == null &&
                reservation!.arrangement!.startDate.isBefore(DateTime.now())) ||
            reservation!.arrangement!.endDate!.isBefore(DateTime.now()))) {
      return BottomAppBar(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: ElevatedButton(
            onPressed: () => showReviewDialog(context),
            child: const Text('Ostavi recenziju',
                style: TextStyle(color: Colors.white)),
          ),
        ),
      );
    }
    return null;
  }

  void showReviewDialog(BuildContext context) {
    final _commentController = TextEditingController();
    final _formKey = GlobalKey<FormState>();
    double rating = 0.0;

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Center(child: Text('Ostavi recenziju')),
          content: Form(
            key: _formKey,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                RatingBar.builder(
                  itemSize: 30,
                  minRating: 1,
                  direction: Axis.horizontal,
                  itemCount: 5,
                  itemPadding: const EdgeInsets.symmetric(horizontal: 4.0),
                  itemBuilder: (context, _) => const Icon(
                    Icons.star,
                    color: Colors.amber,
                  ),
                  onRatingUpdate: (ratingTemp) {
                    rating = ratingTemp;
                  },
                ),
                const SizedBox(height: 5),
                TextField(
                  controller: _commentController,
                  maxLines: 5,
                  minLines: 1,
                  decoration: InputDecoration(labelText: 'Komentar'),
                )
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              child: Text('Otkaži'),
              onPressed: () {
                Navigator.of(context).pop(false);
              },
            ),
            TextButton(
              child: Text('Pošalji'),
              onPressed: () async {
                try {
                  await reservationProvider?.addReview(
                      reservation!.id, rating.toInt());
                  await loadData();
                  ScaffoldMessengerHelper.showCustomSnackBar(
                      context: context,
                      message: "Uspješno ste ostavili recenziju",
                      backgroundColor: Colors.green);
                } catch (error) {
                  ScaffoldMessengerHelper.showCustomSnackBar(
                      context: context,
                      message: "Došlo je do greške: ${error.toString()}",
                      backgroundColor: Colors.red);
                } finally {
                  Navigator.of(context).pop(true);
                }
              },
            ),
          ],
        );
      },
    );
  }

  Color getColor(int statusId) {
    switch (statusId) {
      case 2:
      case 3:
      case 6:
        return Colors.red;
      case 4:
        return Colors.green;
      default:
        return Colors.grey;
    }
  }
}
