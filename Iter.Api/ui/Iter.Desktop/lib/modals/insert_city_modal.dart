import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/city_provider.dart';
import 'package:ui/services/dropdown_provider.dart';

class InsertCityModal extends StatefulWidget {
  const InsertCityModal({super.key, required this.onCompleted, this.id});

  final Function onCompleted;
  final String? id;

  @override
  _InsertCityModalState createState() => _InsertCityModalState();
}

class _InsertCityModalState extends State<InsertCityModal> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};
  List<DropdownModel> countires = [];
  DropdownProvider? _dropdownProvider;
  CityProvider? _cityProvider;
  String? selectedCountry = "";
  bool displayLoader = false;

  @override
  void initState() {
    super.initState();
    _cityProvider = context.read<CityProvider>();
    _dropdownProvider = context.read<DropdownProvider>();

    if (widget.id != null) {
      initialLoad();
    }
    loadCountries();
  }

  Future<void> loadCountries() async {
    var cityListResponse = await _dropdownProvider?.get({
      "dropdownType": DropdownTypes.countries.index.toString(),
    });

    if (cityListResponse != null) {
      List<DropdownModel> cityList = [
        DropdownModel(id: "", name: "Izaberite državu"),
        ...cityListResponse.result
      ];

      setState(() {
        countires = cityList;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
        title: Column(
          children: [
            Text(widget.id == null ? 'Dodaj grad' : 'Uredi grad'),
            const SizedBox(height: 10),
            const Icon(
              Icons.location_city,
              color: Colors.amber,
              size: 50,
            ),
          ],
        ),
        contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
        content: displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Container(
                width: 600.0,
                child: FormBuilder(
                  key: _formKey,
                  initialValue: _initialValue,
                  child: SingleChildScrollView(
                    child: Column(children: [
                      Row(
                        children: [
                          Expanded(
                            child: FormBuilderTextField(
                              name: 'name',
                              decoration:
                                  const InputDecoration(labelText: 'Naziv'),
                              validator: FormBuilderValidators.compose([
                                FormBuilderValidators.required(
                                    errorText: "Polje je obavezno"),
                                FormBuilderValidators.maxLength(30,
                                    errorText: "Neispravan unos"),
                              ]),
                            ),
                          ),
                          SizedBox(width: 60),
                          Expanded(
                            child: DropdownButtonFormField<dynamic>(
                              decoration: const InputDecoration(
                                labelText: 'Izaberite državu',
                              ),
                              value: selectedCountry,
                              items: countires.map((DropdownModel item) {
                                return DropdownMenuItem<dynamic>(
                                  value: item.id,
                                  child: Text(item.name ?? ""),
                                );
                              }).toList(),
                              onChanged: (value) async {
                                setState(() {
                                  selectedCountry = value;
                                });
                              },
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                return null;
                              },
                            ),
                          ),
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
                              backgroundColor:
                                  WidgetStatePropertyAll<Color>(Colors.white70),
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
                    ]),
                  ),
                ),
              ));
  }

  Future<void> submit() async {
    try {
      var request = new Map.from(_formKey.currentState!.value);
      request["countryId"] = selectedCountry;
      if (widget.id == null) {
        await _cityProvider?.insert(request);
      } else {
        await _cityProvider?.update(widget.id, request);
      }
      widget.onCompleted();

      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: widget.id == null ? "Grad uspješno dodan!" : "Grad uspješno uređen!",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    }
  }

  Future<void> initialLoad() async {
    setState(() {
      displayLoader = true;
    });

    var city = await _cityProvider?.getById(widget.id);

    if (city != null) {
      _initialValue = {
        "name": city.name,
      };

      selectedCountry = city.countryId.toString();

      setState(() {
        displayLoader = false;
      });
    }
  }
}
