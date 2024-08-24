import 'dart:convert';
import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:flutter_multi_formatter/formatters/masked_input_formatter.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/image_helper.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/password_change_modal.dart';
import 'package:ui/models/address.dart';
import 'package:ui/models/agency.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/image_model.dart';
import 'package:ui/services/agency_provider.dart';
import 'package:ui/services/auth_storage_provider.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/widgets/layout.dart';
import 'package:ui/widgets/logo.dart';

class EditProfilePage extends StatefulWidget {
  const EditProfilePage({super.key});

  @override
  State<EditProfilePage> createState() => _EditProfilePageState();
}

class _EditProfilePageState extends State<EditProfilePage> {
  final _formKey = GlobalKey<FormBuilderState>();
  Map<String, dynamic> _initialValue = {};
  List<DropdownModel> cities = [];
  DropdownProvider? _dropdownProvider;
  AgencyProvider? _agencyProvider;
  ImageModel? _logo;
  String? selectedCity = "";
  bool displayLoader = false;

  @override
  void initState() {
    super.initState();
    _agencyProvider = context.read<AgencyProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    initialLoad();
    loadCities();
  }

  Future<void> initialLoad() async {
    setState(() {
      displayLoader = true;
    });

    var agencyId = AuthStorageProvider.getAuthData()?["agencyId"];
    var agency = await _agencyProvider?.getById(agencyId);

    if (agency != null) {
      _initialValue = {
        "name": agency.name,
        "contactEmail": agency.contactEmail,
        "contactPhone": agency.contactPhone,
        "website": agency.website,
        "licenseNumber": agency.licenseNumber,
        "street": agency.address?.street,
        "houseNumber": agency.address?.houseNumber,
        "postalCode": agency.address?.postalCode,
      };

      selectedCity = agency!.address!.cityId;
      if (agency.logo != null) {
        _logo = agency.logo;
      }
    }

    setState(() {
      displayLoader = false;
    });
  }

  Future<void> loadCities() async {
    var cityListResponse = await _dropdownProvider?.get({
      "dropdownType": DropdownTypes.cities.index.toString(),
      "countryId": 1 // Bosnia
    });

    if (cityListResponse != null) {
      List<DropdownModel> cityList = [
        DropdownModel(id: "", name: "Izaberite grad"),
        ...cityListResponse.result
      ];

      setState(() {
        cities = cityList;
      });
    }
  }

  Future getImage() async {
    var result = await FilePicker.platform.pickFiles(
      type: FileType.image,
      allowMultiple: false,
    );

    if (result != null && result.files.isNotEmpty) {
      var imagesTemp = await Future.wait(result.files.map((file) async {
        return await ImageHelper.processImage(File(file.path!));
      }));

      setState(() {
        _logo = imagesTemp[0];
      });
    }
  }

  Future<void> submit() async {
    try {
      var agencyId = AuthStorageProvider.getAuthData()?["agencyId"];
      var request = new Map.from(_formKey.currentState!.value);
      request["logo"] = _logo?.toJson();

      request["address"] = Address(
        cityId: selectedCity,
        country: request["country"],
        postalCode: request["postalCode"],
        street: request["street"],
        houseNumber: request["houseNumber"],
      ).toJson();

      await _agencyProvider?.update(agencyId, request);

      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Profil uspješno ažuriran!",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
        displayBackNavigationArrow: false,
        name: "Profil",
        icon: Icons.business,
        body: displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Card(
                child: Padding(
                  padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Expanded(
                        flex: 1,
                        child: Column(
                          children: [
                            if (_logo != null) ...[
                              Padding(
                                padding: const EdgeInsets.fromLTRB(0, 16, 0, 0),
                                child: Image.memory(base64Decode(_logo?.image)),
                              ),
                            ],
                            FormBuilderField(
                              name: 'logo',
                              builder: ((field) {
                                return InputDecorator(
                                  decoration: InputDecoration(
                                      label: const Text('Odaberite logo'),
                                      errorText: field.errorText),
                                  child: ListTile(
                                    leading: const Icon(Icons.photo),
                                    title: const Text("Logo agencije"),
                                    trailing: const Icon(Icons.file_upload),
                                    onTap: getImage,
                                  ),
                                );
                              }),
                            ),
                            SizedBox(height: 20),
                            Container(
                              width: double.infinity,
                              child: ElevatedButton(
                                onPressed: () {
                                  showDialog(
                                    context: context,
                                    builder: (BuildContext context) {
                                      return ChangePasswordModal();
                                    },
                                  );
                                },
                                child: const Text(
                                  'Promijeni lozinku',
                                  style: TextStyle(color: Colors.white),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                      SizedBox(width: 60),
                      Expanded(
                        flex: 3,
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
                                        decoration: const InputDecoration(
                                            labelText: 'Naziv'),
                                        validator:
                                            FormBuilderValidators.compose([
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
                                        decoration: const InputDecoration(
                                            labelText: 'Email'),
                                        validator:
                                            FormBuilderValidators.compose([
                                          FormBuilderValidators.required(
                                              errorText: "Polje je obavezno"),
                                          FormBuilderValidators.maxLength(30,
                                              errorText:
                                                  "Nevažeća email adresa"),
                                          FormBuilderValidators.email(
                                              errorText:
                                                  "Nevažeća email adresa"),
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
                                        inputFormatters: [
                                          MaskedInputFormatter('000-000-0000'),
                                        ],
                                        decoration: const InputDecoration(
                                            labelText: 'Kontakt telefon'),
                                        validator:
                                            FormBuilderValidators.compose([
                                          FormBuilderValidators.required(
                                              errorText: "Polje je obavezno"),
                                          FormBuilderValidators.maxLength(20,
                                              errorText: "Neispravan unos"),
                                          FormBuilderValidators.match(
                                            r'^\d{3}-\d{3}-\d{3,4}$',
                                            errorText: "Neispravan format",
                                          ),
                                        ]),
                                      ),
                                    ),
                                    const SizedBox(width: 60),
                                    Expanded(
                                      child: FormBuilderTextField(
                                        name: 'website',
                                        decoration: const InputDecoration(
                                            labelText: 'Web stranica'),
                                        validator:
                                            FormBuilderValidators.compose([
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
                                        decoration: const InputDecoration(
                                            labelText: 'Broj licence'),
                                        validator:
                                            FormBuilderValidators.compose([
                                          FormBuilderValidators.required(
                                              errorText: "Polje je obavezno"),
                                          FormBuilderValidators.maxLength(20,
                                              errorText: "Neispravan unos"),
                                        ]),
                                      ),
                                    ),
                                    const SizedBox(width: 60),
                                    Spacer()
                                  ],
                                ),
                                const SizedBox(height: 30),
                                Row(
                                  children: [
                                    Expanded(
                                      child: DropdownButtonFormField<dynamic>(
                                        decoration: const InputDecoration(
                                          labelText: 'Izaberite grad',
                                        ),
                                        value: selectedCity,
                                        items: cities.map((DropdownModel item) {
                                          return DropdownMenuItem<dynamic>(
                                            value: item.id,
                                            child: Text(item.name ?? ""),
                                          );
                                        }).toList(),
                                        onChanged: (value) async {
                                          setState(() {
                                            selectedCity = value;
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
                                    const SizedBox(width: 60),
                                    Expanded(
                                      child: FormBuilderTextField(
                                        name: 'postalCode',
                                        decoration: const InputDecoration(
                                            labelText: 'Poštanski broj'),
                                        validator:
                                            FormBuilderValidators.compose([
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
                                        decoration: const InputDecoration(
                                            labelText: 'Ulica'),
                                        validator:
                                            FormBuilderValidators.compose([
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
                                        decoration: const InputDecoration(
                                            labelText: 'Kućni broj'),
                                        validator:
                                            FormBuilderValidators.compose([
                                          FormBuilderValidators.required(
                                              errorText: "Polje je obavezno"),
                                          FormBuilderValidators.maxLength(5,
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
                                            MaterialStatePropertyAll<Color>(
                                                Colors.white70),
                                      ),
                                      child: const Text('Odustani'),
                                    ),
                                    const SizedBox(width: 8),
                                    ElevatedButton(
                                      onPressed: () async {
                                        if (_formKey.currentState
                                                ?.saveAndValidate() ??
                                            false) {
                                          await submit();
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
                    ],
                  ),
                ),
              ));
  }
}
