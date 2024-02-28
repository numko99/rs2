import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/reservation_provider.dart';

class InsertReservationModal extends StatefulWidget {
  const InsertReservationModal(
      {super.key, required this.onCompleted, this.arrangementId});

  final Function onCompleted;
  final String? arrangementId;
  @override
  _InsertReservationModalState createState() => _InsertReservationModalState();
}

class _InsertReservationModalState extends State<InsertReservationModal> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};

  ReservationProvider? _reservationProvider;
  DropdownProvider? _dropdownProvider;
  bool _displayLoader = true;

  String? selectedUser;
  String? selectedAccomodationType;
  List<DropdownModel>? users;
  List<DropdownModel>? prices;

  @override
  void initState() {
    super.initState();
    _reservationProvider = context.read<ReservationProvider>();
    _dropdownProvider = context.read<DropdownProvider>();

    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });
    var usersDropdownTemp = await _dropdownProvider!
        .get({"dropdownType": DropdownTypes.clients.index});

    usersDropdownTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));

    var arrangementPricesTemp = await _dropdownProvider!.get({
      "dropdownType": DropdownTypes.accomodationTypes.index,
      "arrangementId": widget.arrangementId
    });

    arrangementPricesTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));
    setState(() {
      _displayLoader = false;
      users = usersDropdownTemp.result;
      prices = arrangementPricesTemp.result;
    });
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Column(
        children: [
          Text('Dodaj rezervaciju'),
          SizedBox(height: 10),
          Icon(
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
                      Row(
                        children: [
                          Expanded(
                            flex: 1,
                            child: DropdownButtonFormField<dynamic?>(
                                decoration: const InputDecoration(
                                  labelText: 'Izaberite korisnika',
                                ),
                                value: selectedUser,
                                items: users?.map((DropdownModel item) {
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
                                    selectedUser = value;
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
                      if ((prices!.length == 2 && prices?[1].name == null) == false) ...[
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
                                  items: prices?.map((DropdownModel item) {
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
                                      selectedAccomodationType = value;
                                    });
                                  }),
                            ),
                          ],
                        ),
                      ],
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
                      SizedBox(height: 30),
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
      request["clientId"] = selectedUser;
      request["arrangmentId"] = widget.arrangementId;

      if (prices?.length == 2 && prices?[1].name == null) {
        selectedAccomodationType = prices?[1].id;
      }

      request["arrangementPriceId"] = selectedAccomodationType;

      await _reservationProvider?.insert(request);
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
