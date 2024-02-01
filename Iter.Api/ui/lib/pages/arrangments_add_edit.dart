import 'dart:convert';
import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/image_helper.dart';
import 'package:ui/models/arrangement_price.dart';
import 'package:ui/models/accomodation.dart';
import 'package:ui/models/address.dart';
import 'package:ui/models/destination.dart';
import 'package:ui/models/image_model.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';

class ArrangementAddEditPage extends StatefulWidget {
  const ArrangementAddEditPage({super.key});

  @override
  State<ArrangementAddEditPage> createState() => _ArrangementAddEditPageState();
}

class _ArrangementAddEditPageState extends State<ArrangementAddEditPage> {
  final List<GlobalKey<FormBuilderState>> formKeys = [
    GlobalKey<FormBuilderState>(),
    GlobalKey<FormBuilderState>(),
    GlobalKey<FormBuilderState>(),
  ];

  ArrangmentProvider? _arrangmentProvider;
  List<dynamic>? images = [];
  dynamic mainImage;
  bool displayLoader = false;

  List<ArrangementPrice> pricesRows = [];
  List<Destination> destionationsRows = [];

  Map<int, bool> isHotelIncluded = {};

  int? arangmentType = 1;
  int currentStep = 0;

  @override
  void initState() {
    super.initState();
    pricesRows.add(ArrangementPrice());
    destionationsRows.add(Destination());
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _arrangmentProvider =
          Provider.of<ArrangmentProvider>(context, listen: false);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      body: Card(
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(children: [
            const Text('Dodaj aranžman'),
            const SizedBox(height: 10),
            const Icon(
              Icons.card_travel,
              color: Colors.amber,
              size: 50,
            ),
            if (displayLoader)
              const CircularProgressIndicator()
            else
              Padding(
                  padding: const EdgeInsets.fromLTRB(0, 15, 30.0, 40),
                  child: Stepper(
                    currentStep: currentStep,
                    onStepContinue: currentStep < 2 ? increaseStep : submitData,
                    onStepCancel: currentStep > 0
                        ? () => setState(() => currentStep--)
                        : null,
                    controlsBuilder:
                        (BuildContext context, ControlsDetails details) {
                      return Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: <Widget>[
                          ElevatedButton(
                            onPressed: details.onStepContinue,
                            child: Text(currentStep < 2 ? 'Dalje' : 'Završi',
                                style: TextStyle(
                                  fontSize: 15,
                                  color: Colors.white,
                                )),
                            style: ElevatedButton.styleFrom(
                                backgroundColor: Colors.amber),
                          ),
                          SizedBox(width: 10),
                          ElevatedButton(
                            onPressed: details.onStepCancel,
                            child: const Text('Nazad',
                                style: TextStyle(
                                    fontSize: 15, color: Colors.white)),
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.grey,
                            ),
                          ),
                        ],
                      );
                    },
                    steps: [
                      Step(
                        title: const Text('Osnovni podaci'),
                        content: FormBuilder(
                            key: formKeys[0], child: _buildBasicInfoForm()),
                        isActive: currentStep == 0,
                      ),
                      Step(
                        title: const Text('Slike i Opis'),
                        content: FormBuilder(
                            key: formKeys[1],
                            child: _buildImagesAndDescriptionForm()),
                        isActive: currentStep == 1,
                      ),
                      Step(
                        title: const Text('Destinacije i smještaji'),
                        content: FormBuilder(
                            key: formKeys[2], child: _buildDestinationForm()),
                        isActive: currentStep == 2,
                      ),
                    ],
                  )),
          ]),
        ),
      ),
    );
  }

  Future<void> submitData() async {
    try {
      if (formKeys[currentStep].currentState?.saveAndValidate() ?? false) {
        var basicDataFormData = Map.from(formKeys[0].currentState!.value);
        var processedBasicData = processBasicData(basicDataFormData);

        var descriptionAndImagesFormData =
            Map.from(formKeys[1].currentState!.value);
        var procesedDescriptionAndImagesFormData =
            await processDescriptionAndImagesData(descriptionAndImagesFormData);

        var destinatiosFormData = Map.from(formKeys[2].currentState!.value);
        var procesedDestinatiosFormData =
            processDestinationsData(destinatiosFormData);

        var finalFormData = {
          ...processedBasicData,
          ...procesedDescriptionAndImagesFormData
        };
        finalFormData["destinations"] = procesedDestinatiosFormData;
        await _arrangmentProvider?.insert(finalFormData);

      }
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
        content: Center(child: Text("Došlo je do greške na serveru")),
        backgroundColor: Colors.red,
      ));
      print(error);
    } finally {
      setState(() => displayLoader = false);
    }
  }

  List<Destination> processDestinationsData(Map<dynamic, dynamic> formData) {
    List<Destination> destinationList = [];

    int index = 0;
    while (formData.containsKey('destination_$index')) {
      var destination = Destination(
          city: formData['destination_$index'],
          country: formData['country_$index'],
          arrivalDate: formData['arrivalDateTime_$index'],
          departureDate: formData['departureDateTime_$index'],
          isOneDayTrip: formData['isHotelIncluded_$index'] == 'false',
          accommodation: Accomodation(
            hotelName: formData['hotelName_$index'],
            checkInDate: formData['arrivalDateTime_$index'],
            checkOutDate: formData['departureDateTime_$index'],
            hotelAddress: Address(
                street: formData['street_$index'],
                houseNumber: formData['houseNumber_$index'],
                city: formData['city_$index'],
                postalCode: formData['postalCode_$index'],
                country: formData['country_$index']
              ),
            )
        );

      destinationList.add(destination);
      index++;
    }

    return destinationList;
  }

  Future<Map<dynamic, dynamic>> processDescriptionAndImagesData(
      Map<dynamic, dynamic> formData) async {
    var mainImageTemp = await ImageHelper.processImage(mainImage);
    formData["mainImage"] = mainImageTemp.toJson();

    List<ImageModel> imagesTemp = [];
    if (images != null) {
      for (var image in images!) {
        var imageTemp = await ImageHelper.processImage(image);
        imagesTemp.add(imageTemp);
      }
    }

    formData["images"] = imagesTemp.map((image) => image.toJson()).toList();
    return formData;
  }

  Map<dynamic, dynamic> processBasicData(Map<dynamic, dynamic> formData) {
    List<Map<String, dynamic>> prices = [];
    int index = 0;

    while (formData.containsKey('accommodationType_$index')) {
      var accommodationType = formData['accommodationType_$index'];
      var priceKey = 'price_$index';
      var price = double.tryParse(formData[priceKey].toString()) ?? 0;

      var priceData = ArrangementPrice(
          accommodationType: accommodationType, price: price);

      prices.add(priceData.toJson());

      formData.remove('accommodationType_$index');
      formData.remove('price_$index');

      formData["prices"] = prices;
      index++;
    }

    DateTime startDate = formData["startDate"]; 
    DateTime endDate = formData["endDate"];
    formData["startDate"] = startDate.toIso8601String();
    formData["endDate"] = endDate.toIso8601String();
    formData["prices"] = prices;

    return formData;
  }

  Future<void> increaseStep() async {
    if (formKeys[currentStep].currentState?.saveAndValidate() ?? false) {
      setState(() {
        currentStep = currentStep + 1;
      });
    }
  }

  Future getImages(bool allowMultiple) async {
    var result = await FilePicker.platform.pickFiles(
      type: FileType.image,
      allowMultiple: allowMultiple,
    );

    if (result != null && result.files.isNotEmpty) {
      List<File> files = result.files.map((file) => File(file.path!)).toList();
      setState(() {
        if (allowMultiple) {
          images?.addAll(files);
        } else {
          mainImage = files[0];
        }
      });
    }
  }

  Widget _buildBasicInfoForm() {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            Expanded(
              child: Row(
                children: [
                  Radio(
                      value: 1,
                      groupValue: arangmentType,
                      onChanged: (value) {
                        setState(() {
                          arangmentType = value;
                        });
                      }),
                  Expanded(
                    child: Text('Višednevno putovanje'),
                  )
                ],
              ),
            ),
            Expanded(
              child: Row(
                children: [
                  Radio(
                      value: 2,
                      groupValue: arangmentType,
                      onChanged: (value) {
                        setState(() {
                          arangmentType = value;
                        });
                      }),
                  Expanded(child: Text('Jednodnevni izlet'))
                ],
              ),
            ),
          ],
        ),
        SizedBox(height: 20),
        Row(
          children: [
            Expanded(
              child: FormBuilderTextField(
                name: 'name',
                decoration: const InputDecoration(labelText: 'Naziv'),
                validator: FormBuilderValidators.compose([
                  FormBuilderValidators.required(
                      errorText: "Polje je obavezno"),
                  FormBuilderValidators.maxLength(100,
                      errorText: "Neispravan unos"),
                ]),
              ),
            ),
            const SizedBox(width: 60),
            Expanded(
              child: FormBuilderDateTimePicker(
                name: 'startDate',
                decoration: const InputDecoration(labelText: 'Datum polaska'),
                validator: FormBuilderValidators.compose([
                  FormBuilderValidators.required(
                      errorText: "Polje je obavezno"),
                ]),
                inputType: InputType.date,
              ),
            ),
            const SizedBox(width: 60),
            arangmentType == 1
                ? Expanded(
                    child: FormBuilderDateTimePicker(
                      name: 'endDate',
                      decoration:
                          const InputDecoration(labelText: 'Datum povratka'),
                      validator: FormBuilderValidators.compose([
                        FormBuilderValidators.required(
                            errorText: "Polje je obavezno"),
                      ]),
                      inputType: InputType.date,
                    ),
                  )
                : Expanded(
                    child: Row(
                      children: [
                        Expanded(
                          child: FormBuilderTextField(
                            name: 'price',
                            decoration: const InputDecoration(
                              labelText: 'Cijena',
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
                                fontSize: 16, fontWeight: FontWeight.w400)),
                      ],
                    ),
                  )
          ],
        ),
        const SizedBox(height: 20),
        if (arangmentType == 1)
          Column(
            children: [
              Column(
                children: List.generate(pricesRows.length, (int index) {
                  return Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      Expanded(
                        child: FormBuilderTextField(
                          name: 'accommodationType_$index',
                          decoration: InputDecoration(
                              labelText: index == 0 ? 'Tip smještaja' : null),
                          validator: FormBuilderValidators.compose([
                            FormBuilderValidators.required(
                                errorText: "Polje je obavezno"),
                            FormBuilderValidators.maxLength(100,
                                errorText: "Neispravan unos"),
                          ]),
                        ),
                      ),
                      const SizedBox(width: 60),
                      Expanded(
                        child: Row(
                          children: [
                            Expanded(
                              flex: 9,
                              child: FormBuilderTextField(
                                name: 'price_$index',
                                decoration: InputDecoration(
                                  labelText: index == 0 ? 'Cijena' : null,
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
                            Expanded(
                              flex: 1,
                              child: Text('KM',
                                  style: TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.w400)),
                            ),
                          ],
                        ),
                      ),
                      SizedBox(width: 60),
                      Expanded(
                        child: Align(
                          alignment: Alignment.centerLeft,
                          child: IconButton(
                            icon: const Icon(Icons.close),
                            onPressed: () {
                              setState(() {
                                if (pricesRows.length != 1) {
                                  pricesRows.removeAt(index);
                                }
                              });
                            },
                          ),
                        ),
                      ),
                    ],
                  );
                }),
              ),
              const SizedBox(height: 20),
              Row(
                children: [
                  InkWell(
                    onTap: () {
                      setState(() {
                        pricesRows.add(ArrangementPrice());
                      });
                    },
                    child: const Row(
                      mainAxisSize: MainAxisSize.min,
                      children: <Widget>[
                        Icon(
                          Icons.add_circle_rounded,
                          color: Colors.amber,
                          size: 30,
                        ),
                        Padding(
                          padding: EdgeInsets.only(left: 8),
                          child: Text(
                            'Dodaj smještaj',
                            style: TextStyle(
                              color: Colors.amber,
                              fontSize: 16,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
      ],
    );
  }

  Widget _buildImagesAndDescriptionForm() {
    return Column(children: [
      Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Padding(
              padding: const EdgeInsets.fromLTRB(0, 6, 0, 0),
              child: FormBuilderTextField(
                  name: 'description',
                  maxLines: 10,
                  minLines: 1,
                  decoration: const InputDecoration(labelText: 'Opis'),
                  validator: FormBuilderValidators.compose([
                    FormBuilderValidators.required(
                        errorText: "Polje je obavezno"),
                  ])),
            ),
          ),
        ],
      ),
      const SizedBox(height: 20),
      Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              children: [
                FormBuilderField(
                  name: 'mainImage',
                  builder: ((field) {
                    return InputDecorator(
                      decoration: InputDecoration(
                          label: Text('Odaberite pozadinu'),
                          errorText: field.errorText),
                      child: ListTile(
                        leading: Icon(Icons.photo),
                        title: Text("Glavna slika"),
                        trailing: Icon(Icons.file_upload),
                        onTap: () => getImages(false),
                      ),
                    );
                  }),
                ),
                if (mainImage != null) ...[
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Container(
                        padding: EdgeInsets.fromLTRB(0, 15, 0, 0),
                        width: 200,
                        height: 200,
                        child: mainImage is File
                            ? Image.file(mainImage!)
                            : mainImage,
                      ),
                      IconButton(
                        icon: Icon(Icons.close),
                        onPressed: () {
                          setState(() {
                            mainImage = null;
                          });
                        },
                      ),
                    ],
                  ),
                ]
              ],
            ),
          ),
          const SizedBox(width: 60),
          Expanded(
              flex: 2,
              child: Column(children: [
                FormBuilderField(
                  name: 'images',
                  builder: ((field) {
                    return InputDecorator(
                      decoration: InputDecoration(
                          label: Text('Odaberite slike putovanja'),
                          errorText: field.errorText),
                      child: ListTile(
                        leading: Icon(Icons.photo),
                        title: Text("Slike"),
                        trailing: Icon(Icons.file_upload),
                        onTap: () => getImages(true),
                      ),
                    );
                  }),
                ),
                if (images != null && images!.isNotEmpty) ...[
                  Wrap(
                    alignment: WrapAlignment.start,
                    spacing: 8.0,
                    runSpacing: 8.0,
                    children: List<Widget>.generate(images!.length, (index) {
                      return Row(
                        mainAxisSize: MainAxisSize.min,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Container(
                            padding: EdgeInsets.fromLTRB(0, 15, 0, 0),
                            width: 100,
                            height: 100,
                            child:
                                Image.file(images![index], fit: BoxFit.cover),
                          ),
                          IconButton(
                            icon: Icon(Icons.close),
                            onPressed: () {
                              setState(() {
                                images?.removeAt(index);
                              });
                            },
                          ),
                        ],
                      );
                    }),
                  ),
                ]
              ])),
        ],
      ),
      const SizedBox(height: 20),
    ]);
  }

  Widget _buildDestinationForm() {
    return Column(
      children: [
        Column(
            children: List.generate(destionationsRows.length, (int index) {
          return Container(
            padding: EdgeInsets.fromLTRB(0, 10, 0, 30),
            child: Column(
              children: [
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Align(
                        alignment: Alignment.bottomLeft,
                        child: Text("${index + 1}. destinacija",
                            style:
                                TextStyle(color: Colors.amber, fontSize: 15))),
                    IconButton(
                      icon: const Icon(Icons.close),
                      onPressed: () {
                       setState(() {
                          if (destionationsRows.length != 1) {
                            destionationsRows.removeAt(index);
                          }
                        });
                      },
                    )
                  ],
                ),
                Row(
                  children: [
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'destination_$index',
                        decoration: const InputDecoration(labelText: 'Destinacija'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(100,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: FormBuilderTextField(
                        name: 'country_$index',
                        decoration: const InputDecoration(labelText: 'Država'),
                        validator: FormBuilderValidators.compose([
                          FormBuilderValidators.required(
                              errorText: "Polje je obavezno"),
                          FormBuilderValidators.maxLength(100,
                              errorText: "Neispravan unos"),
                        ]),
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: Row(
                        children: [
                          Expanded(
                            child: FormBuilderDateTimePicker(
                              name: 'arrivalDateTime_$index',
                              decoration: const InputDecoration(
                                  labelText: 'Vrijeme dolaska'),
                              validator: FormBuilderValidators.compose([
                                FormBuilderValidators.required(
                                    errorText: "Polje je obavezno"),
                              ]),
                              inputType: InputType.both,
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            child: FormBuilderDateTimePicker(
                              name: 'departureDateTime_$index',
                              decoration: const InputDecoration(
                                  labelText: 'Vrijeme odlaska'),
                              validator: FormBuilderValidators.compose([
                                FormBuilderValidators.required(
                                    errorText: "Polje je obavezno"),
                              ]),
                              inputType: InputType.both,
                            ),
                          )
                        ],
                      ),
                    )
                  ],
                ),
                const SizedBox(height: 20),
                FormBuilderCheckbox(
                  name: 'isHotelIncluded_$index',
                  initialValue: isHotelIncluded[index] ?? false,
                  title: Text('Uključen hotel', style: TextStyle(fontSize: 15)),
                  onChanged: (bool? value) {
                    setState(() {
                      isHotelIncluded[index] = value ?? false;
                    });
                  },
                ),
                if (isHotelIncluded[index] ?? false)
                  Column(
                    children: [
                      Row(
                        children: [
                          Expanded(
                            child: FormBuilderTextField(
                              name: 'hotelName_$index',
                              decoration: const InputDecoration(
                                  labelText: 'Naziv hotela'),
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
                              name: 'city_$index',
                              decoration:
                                  const InputDecoration(labelText: 'Grad'),
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
                              name: 'postalCode_$index',
                              decoration: const InputDecoration(
                                  labelText: 'Poštanski broj'),
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
                              name: 'street_$index',
                              decoration:
                                  const InputDecoration(labelText: 'Ulica'),
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
                              name: 'houseNumber_$index',
                              decoration: const InputDecoration(
                                  labelText: 'Kućni broj'),
                              validator: FormBuilderValidators.compose([
                                FormBuilderValidators.required(
                                    errorText: "Polje je obavezno"),
                                FormBuilderValidators.maxLength(5,
                                    errorText: "Neispravan unos"),
                              ]),
                            ),
                          ),
                          const SizedBox(width: 60),
                          Spacer()
                        ],
                      ),
                    ],
                  ),
              ],
            ),
          );
        })),
        Row(
          children: [
            InkWell(
              onTap: () {
                setState(() {
                  destionationsRows.add(Destination());
                });
              },
              child: const Row(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  Icon(
                    Icons.add_circle_rounded,
                    color: Colors.amber,
                    size: 30,
                  ),
                  Padding(
                    padding: EdgeInsets.only(left: 8),
                    child: Text(
                      'Dodaj destinaciju',
                      style: TextStyle(
                        color: Colors.amber,
                        fontSize: 16,
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ],
    );
  }
}
