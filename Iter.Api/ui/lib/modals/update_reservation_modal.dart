import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/reservation.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/reservation_provider.dart';

class UpdateReservationModal extends StatefulWidget {
  const UpdateReservationModal(
      {super.key, required this.onCompleted, this.reservationId});

  final Function onCompleted;
  final String? reservationId;
  @override
  UpdateReservationModalState createState() => UpdateReservationModalState();
}

class UpdateReservationModalState extends State<UpdateReservationModal> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};

  ReservationProvider? _reservationProvider;
  ArrangmentProvider? _arrangmentProvider;
  DropdownProvider? _dropdownProvider;
  bool _displayLoader = true;
  Reservation? reservation;

  String? selectedAccomodationType;
  List<DropdownModel>? accomodationTypes;

  String? selectedReservationStatus;
  List<DropdownModel>? reservationStatuses;

  double? totalPrice;

  @override
  void initState() {
    super.initState();
    _reservationProvider = context.read<ReservationProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    _arrangmentProvider = context.read<ArrangmentProvider>();

    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });

    var reservationTemp =
        await _reservationProvider?.getById(widget.reservationId);

    var arrangementPricesTemp = await _dropdownProvider!.get({
      "dropdownType": DropdownTypes.accomodationTypes.index,
      "arrangementId": reservationTemp?.arrangement.id
    });
    arrangementPricesTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));

    var reservationStatusesTemp = await _dropdownProvider!.get({
      "dropdownType": DropdownTypes.reservationStatus.index,
    });
    reservationStatusesTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));

    setState(() {
      _displayLoader = false;
      reservation = reservationTemp;
      accomodationTypes = arrangementPricesTemp.result;
      reservationStatuses = reservationStatusesTemp.result;
      selectedAccomodationType = reservation?.arrangementPriceId;
      selectedReservationStatus = reservation?.reservationStatusId.toString();
      totalPrice = reservation?.arrangementPrice?.price;
      _initialValue = {
        "departurePlace": reservation?.departurePlace,
        "totalPaid": reservation?.totalPaid.toString(),
        "reminder": reservation?.reminder
      };
    });
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Column(
        children: [
          const Text('Uređivanje rezervacije'),
          Text(reservation?.reservationNumber ?? "", style: const TextStyle(fontSize: 30)),
          const SizedBox(height: 10),
          const Icon(
            Icons.list_alt,
            color: Colors.amber,
            size: 50,
          ),
        ],
      ),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: _displayLoader
          ? const Center(child: Column(children: [CircularProgressIndicator()]))
          : Container(
              width: 600.0,
              child: FormBuilder(
                key: _formKey,
                initialValue: _initialValue,
                child: SingleChildScrollView(
                  child: Column(
                    children: [
                      const SizedBox(height: 15),
                      Row(
                        children: [
                          Expanded(
                              child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                  "${reservation?.user?.firstName} ${reservation?.user?.lastName}", style: const TextStyle(fontSize: 18))
                            ],
                          )),
                        ],
                      ),
                      if ((accomodationTypes!.length == 2 &&
                              accomodationTypes?[1].name == null) ==
                          false) ...[
                        const SizedBox(height: 30),
                        Row(
                          children: [
                            Expanded(
                              flex: 1,
                              child: DropdownButtonFormField<dynamic?>(
                                  decoration: const InputDecoration(
                                    labelText: 'Izaberite tip smještaja',
                                  ),
                                  value: selectedAccomodationType,
                                  items: accomodationTypes
                                      ?.map((DropdownModel item) {
                                    return DropdownMenuItem<dynamic>(
                                      value: item.id,
                                      child: Text(item.name ?? ""),
                                    );
                                  }).toList(),
                                  validator: (value) {
                                    if (value == null) {
                                      return 'Polje je obavezno';
                                    }
                                    return null;
                                  },
                                  onChanged: (value) async{
                                      var totalPriceTemp = await _arrangmentProvider
                                        ?.getArrangementPrice(value);
                                    setState(() {
                                      totalPrice = totalPriceTemp?.price;
                                      selectedAccomodationType = value;
                                    });
                                  }),
                            ),
                            const SizedBox(width: 60),
                            Expanded(
                              child: FormBuilderTextField(
                                name: 'departurePlace',
                                decoration: const InputDecoration(
                                    labelText: 'Grad polaska'),
                                validator: FormBuilderValidators.compose([
                                  FormBuilderValidators.required(
                                      errorText: "Polje je obavezno"),
                                  FormBuilderValidators.maxLength(30,
                                      errorText: "Neispravan unos")
                                ]),
                              ),
                            ),
                          ],
                        ),
                      ],
                      const SizedBox(height: 30),
                      Row(
                        crossAxisAlignment: CrossAxisAlignment.end,
                        children: [
                          Expanded(
                            child: Row(
                              crossAxisAlignment: CrossAxisAlignment.end,
                              children: [
                                Expanded(
                                  child: FormBuilderTextField(
                                    name: 'totalPaid',
                                    decoration: const InputDecoration(
                                      labelText: 'Uplaćeno',
                                    ),
                                    keyboardType: TextInputType.number,
                                    validator: FormBuilderValidators.compose([
                                      FormBuilderValidators.required(
                                          errorText: "Polje je obavezno"),
                                      FormBuilderValidators.numeric(
                                          errorText: "Unesite validan broj"),
                                    ]),
                                  ),
                                ),
                                const Text('KM',
                                    style: TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w400)),
                              ],
                            ),
                          ),
                           Expanded(
                            child: Center(child: Text("Cijena aranžmana: $totalPrice KM"))
                          ),
                        ],
                      ),
                      const SizedBox(height: 30),
                      Row(
                        children: [
                          Expanded(
                            flex: 1,
                            child: DropdownButtonFormField<dynamic?>(
                                decoration: const InputDecoration(
                                  labelText: 'Izaberite status rezervacije',
                                ),
                                value: selectedReservationStatus,
                                items: reservationStatuses
                                    ?.map((DropdownModel item) {
                                  return DropdownMenuItem<dynamic>(
                                    value: item.id,
                                    child: Text(item.name ?? ""),
                                  );
                                }).toList(),
                                validator: (value) {
                                  if (value == null) {
                                    return 'Polje je obavezno';
                                  }
                                  return null;
                                },
                                onChanged: (value) {
                                  setState(() {
                                    selectedReservationStatus = value;
                                  });
                                }),
                          ),
                        ],
                      ),
                      const SizedBox(height: 30),
                      Row(
                        children: [
                          Expanded(
                              child: FormBuilderTextField(
                                  name: 'reminder',
                                  maxLines: 5,
                                  minLines: 1,
                                  decoration: const InputDecoration(
                                      labelText: 'Napomena'))),
                        ],
                      ),
                      const SizedBox(height: 30),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          ElevatedButton(
                            onPressed: () {
                              Navigator.of(context).pop();
                            },
                            style: const ButtonStyle(
                              backgroundColor: MaterialStatePropertyAll<Color>(
                                  Colors.white70),
                            ),
                            child: const Text('Odustani'),
                          ),
                          const SizedBox(width: 8),
                          ElevatedButton(
                            onPressed: () async {
                              if (_formKey.currentState?.saveAndValidate() ??
                                  false) {
                                await submit();

                                Navigator.of(context).pop();
                              }
                            },
                            child: const Text(
                              'Sačuvaj',
                              style: TextStyle(color: Colors.white),
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
              ),
            ),
    );
  }

  Future<void> submit() async {
    try {
      var request = Map.from(_formKey.currentState!.value);

      if (accomodationTypes?.length == 2 &&
          accomodationTypes?[1].name == null) {
        selectedAccomodationType = accomodationTypes?[1].id;
      }

      request["arrangementPriceId"] = selectedAccomodationType;
      request["reservationStatusId"] = selectedReservationStatus;
      request["totalPaid"] = double.parse(request["totalPaid"]);

      await _reservationProvider?.update(reservation?.id, request);
      widget.onCompleted();

      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Rezervacija uspješno dodana!",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    }
  }
}
