import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:flutter_typeahead/flutter_typeahead.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/enums/roles.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/user.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/reservation_provider.dart';
import 'package:ui/services/user_provider.dart';

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
  final TextEditingController _typeAheadController = TextEditingController();

  ReservationProvider? _reservationProvider;
  DropdownProvider? _dropdownProvider;
  UserProvider? _userProvider;
  bool _displayLoader = true;

  String? selectedUser;
  String? selectedAccomodationType;
  String? selectedDepartureCityId;
  List<DropdownModel>? users;
  List<DropdownModel>? prices;
  List<DropdownModel>? cities;

  @override
  void initState() {
    super.initState();
    _reservationProvider = context.read<ReservationProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    _userProvider = context.read<UserProvider>();

    initialLoad();
  }

  Future<List<User>> getUsers(String filter) async {

    setState(() {
      _displayLoader = false;
    });

    var searchResult = await _userProvider?.get({
      "currentPage": 1,
      "pageSize": 10,
      "name": filter,
      "roleId": Roles.client.index + 1
    });

    setState(() {
      _displayLoader = false;
    });

    return searchResult!.result;
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

     var citiesTemp = await _dropdownProvider!.get({
      "dropdownType": DropdownTypes.cities.index,
      "countryId": 1
    });

    arrangementPricesTemp.result
        .insert(0, DropdownModel(id: null, name: "Nije odabrano"));
    setState(() {
      _displayLoader = false;
      users = usersDropdownTemp.result;
      prices = arrangementPricesTemp.result;
      cities = citiesTemp.result;
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
                              child: TypeAheadField<User>(
                                controller: _typeAheadController,
                                suggestionsCallback: (pattern) async {
                                  setState(() {
                                    selectedUser = null;
                                  });
                                  if (pattern.length >= 1){
                                      return await getUsers(pattern);
                                  }
                                },
                                loadingBuilder: (context) =>
                                    const Text('Pretraga...'),
                                errorBuilder: (context, error) =>
                                    const Text('Došlo je do greške!'),
                                emptyBuilder: (context) =>
                                    const Text('Nema pronađenih rezultata!'),

                                builder: (context, controller, focusNode) {
                                  return TextFormField(
                                      controller: controller,
                                      focusNode: focusNode,
                                      autofocus: true,
                                      validator: (value) {
                                        if (value!.isEmpty || selectedUser == null) {
                                          return 'Polje je obavezno'; 
                                        } else {
                                          return null;
                                        }
                                      },
                                      decoration: const InputDecoration(
                                        labelText: 'Korisnik',
                                      ));
                                },
                                itemBuilder: (context, user) {
                                  return ListTile(
                                    title: Text("${user.firstName!} ${user.lastName!}"),
                                  );
                                },
                                onSelected: (user) {
                                  _typeAheadController.text = "${user.firstName!} ${user.lastName!}";
                                  selectedUser = user.clientId;
                                },
                              )),
                          const SizedBox(width: 60),
                          Expanded(
                            flex: 1,
                            child: DropdownButtonFormField<dynamic?>(
                                decoration: const InputDecoration(
                                  labelText: 'Izaberite mjesto polaska',
                                ),
                                value: selectedDepartureCityId,
                                items: cities?.map((DropdownModel item) {
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
                                    selectedDepartureCityId = value;
                                  });
                                }),
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
      request["clientId"] = selectedUser;
      request["arrangmentId"] = widget.arrangementId;
      request["reminder"] = request["reminder"] ?? "";

      if (prices?.length == 2 && prices?[1].name == null) {
        selectedAccomodationType = prices?[1].id;
      }

      request["arrangementPriceId"] = selectedAccomodationType;
      request["departureCityId"] = int.parse(selectedDepartureCityId!);

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
