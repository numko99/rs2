import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/services/country_provider.dart';

class InsertCountryModal extends StatefulWidget {
  const InsertCountryModal({super.key, required this.onCompleted, this.id});

  final Function onCompleted;
  final String? id;

  @override
  _InsertCountryModalState createState() => _InsertCountryModalState();
}

class _InsertCountryModalState extends State<InsertCountryModal> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};
  CountryProvider? _countryProvider;
  bool displayLoader = false;

  @override
  void initState() {
    super.initState();
    _countryProvider = context.read<CountryProvider>();

    if (widget.id != null) {
      initialLoad();
    }
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
        title: Column(
          children: [
            Text(widget.id == null ? 'Dodaj državu' : 'Uredi državu'),
            const SizedBox(height: 10),
            const Icon(
              Icons.flag,
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
      if (widget.id == null) {
        await _countryProvider?.insert(request);
      } else {
        await _countryProvider?.update(widget.id, request);
      }
      widget.onCompleted();

      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: widget.id == null ? "Država uspješno dodana!" : "Država uspješno uređena!",
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

    var country = await _countryProvider?.getById(widget.id);

    if (country != null) {
      _initialValue = {
        "name": country.name,
      };

      setState(() {
        displayLoader = false;
      });
    }
  }
}
