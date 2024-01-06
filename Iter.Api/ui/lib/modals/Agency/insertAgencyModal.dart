import 'dart:convert';
import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/models/agency.dart';

import '../../services/agency_provider.dart';

class InsertAgencyModal extends StatefulWidget {
  const InsertAgencyModal({super.key, required this.onCompleted, this.agency});

  final Function onCompleted;
  final Agency? agency;

  @override
  _InsertAgencyModalState createState() => _InsertAgencyModalState();
}

class _InsertAgencyModalState extends State<InsertAgencyModal> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};

  AgencyProvider? _agencyProvider;
  dynamic? _image;

  @override
  void initState() {
    super.initState();

    if (widget.agency != null) {
      _initialValue = {
        "name": widget.agency?.name,
        "contactEmail": widget.agency?.contactEmail,
        "contactPhone": widget.agency?.contactPhone,
        "website": widget.agency?.website,
        "licenseNumber": widget.agency?.licenseNumber,
        "logo": widget.agency?.logo,
        "street": widget.agency?.address?.street,
        "houseNumber": widget.agency?.address?.houseNumber,
        "city": widget.agency?.address?.city,
        "postalCode": widget.agency?.address?.postalCode,
        "country": widget.agency?.address?.country,
      };
      if (widget.agency?.logo != null) {
        _image = Image.memory(base64Decode(widget.agency!.logo!));
      }
    }

    _agencyProvider = context.read<AgencyProvider>();
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Column(
        children: [
          Text('Dodaj agenciju'),
          SizedBox(height: 10),
          Icon(
            Icons.card_travel,
            color: Colors.amber,
            size: 50,
          ),
        ],
      ),
      contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
      content: Container(
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
                      child: FormBuilderTextField(
                        name: 'name',
                        decoration: const InputDecoration(labelText: 'Naziv'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'contactEmail',
                        decoration: const InputDecoration(labelText: 'Email'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Nevažeća email adresa"),
                          FormBuilderValidators.email(
                              errorText: "Nevažeća email adresa"),
                        ]),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 10),
                Row(
                  children: [
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'contactPhone',
                        decoration:
                            const InputDecoration(labelText: 'Kontakt telefon'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(20,
                              errorText: "Neispravan unos"),
                          FormBuilderValidators.match(r'^[0-9+\-]+$',
                              errorText: "Neispravan format"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'website',
                        decoration:
                            const InputDecoration(labelText: 'Web stranica'),
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
                const SizedBox(height: 10),
                Row(
                  children: [
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'licenseNumber',
                        decoration:
                            const InputDecoration(labelText: 'Broj licence'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(20,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'city',
                        decoration: const InputDecoration(labelText: 'Grad'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    )
                  ],
                ),
                const SizedBox(height: 10),
                Row(
                  children: [
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'country',
                        decoration: const InputDecoration(labelText: 'Država'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'postalCode',
                        decoration:
                            const InputDecoration(labelText: 'Poštanski broj'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(10,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 10),
                Row(
                  children: [
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'street',
                        decoration: const InputDecoration(labelText: 'Ulica'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(30,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'houseNumber',
                        decoration:
                            const InputDecoration(labelText: 'Kućni broj'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(5,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                  ],
                ),
                Row(
                  children: [
                    Expanded(
                        child: FormBuilderField(
                      name: 'logo',
                      builder: ((field) {
                        return InputDecorator(
                          decoration: InputDecoration(
                              label: Text('Odaberite logo'),
                              errorText: field.errorText),
                          child: ListTile(
                            leading: Icon(Icons.photo),
                            title: Text("Logo agencije"),
                            trailing: Icon(Icons.file_upload),
                            onTap: getImage,
                          ),
                        );
                      }),
                    )),
                    if (_image != null) ...[
                      const SizedBox(width: 60),
                      Expanded(
                        child: Stack(
                          children: [
                            Center(
                              child: Container(
                                padding: EdgeInsets.fromLTRB(0, 15, 0, 0),
                                width: 100,
                                height: 100,
                                child: _image is File
                                    ? Image.file(_image!)
                                    : _image,
                              ),
                            ),
                            Positioned(
                              top: 0,
                              right: 0,
                              child: IconButton(
                                icon: Icon(Icons.close),
                                onPressed: () {
                                  setState(() {
                                    _image = null;
                                  });
                                },
                              ),
                            ),
                          ],
                        ),
                      ),
                    ]
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
                            MaterialStatePropertyAll<Color>(Colors.white70),
                      ),
                      child: const Text('Odustani'),
                    ),
                    const SizedBox(width: 8),
                    ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState?.saveAndValidate() ?? false) {
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
      var request = new Map.from(_formKey.currentState!.value);
      request["Logo"] = base64Encode(_image.readAsBytesSync() ?? _image); // TODO FIX WHEN EDIT AGENCY BUT NOT SELECTING NEW IMAGE

      if (widget.agency?.id == null) {
        await _agencyProvider?.insert(request);
      } else {
        await _agencyProvider?.update(widget.agency?.id, request);
      }
      widget.onCompleted();

      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
        content: Center(child: Text("Nova agencija uspješno dodana!")),
        backgroundColor: Colors.green,
      ));
    } catch (error) {
      print('Error loading data: $error');

      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
        content: Center(
            child: Text("Došlo je do greške prilikom dodavanja agencije.")),
        backgroundColor: Colors.red,
      ));
    }
  }

  Future getImage() async {
    var result = await FilePicker.platform.pickFiles(type: FileType.image);
    // print(result?.names);
    if (result != null && result.files.single.path != null) {
      setState(() {
        _image = File(result.files.single.path!);
      });
    }
  }
}
