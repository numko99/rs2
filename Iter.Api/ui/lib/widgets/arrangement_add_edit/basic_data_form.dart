import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:ui/models/accomodation_type_form_controllers.dart';

class BasicDataFormPage extends StatefulWidget {
  const BasicDataFormPage({super.key, required this.controllers, required this.setArrangementType, this.initialArrangementType});

  final AccomodationTypeFormControllers controllers;
  final Function(int?) setArrangementType;
  final int? initialArrangementType;

  @override
  State<BasicDataFormPage> createState() => _BasicDataFormPageState();
}

class _BasicDataFormPageState extends State<BasicDataFormPage> {
  int? arangmentType;
  @override
  void initState() {
    super.initState();
    
    arangmentType = widget.initialArrangementType;
    if (widget.controllers.accomodationTypes.isEmpty){
      addArrangementPrices();
    }
  }

  @override
  Widget build(BuildContext context) {
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
                          widget.setArrangementType(value);
                      }),
                  const Expanded(
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
                        widget.setArrangementType(value);
                      }),
                  const Expanded(child: Text('Jednodnevni izlet'))
                ],
              ),
            ),
          ],
        ),
        const SizedBox(height: 20),
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
                children: List.generate(
                    widget.controllers.accomodationTypes.length, (int index) {
                  return Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: [
                      Expanded(
                        child: TextFormField(
                            controller:
                                widget.controllers.accomodationTypes[index],
                            decoration: InputDecoration(
                                labelText: index == 0 ? 'Tip smještaja' : null),
                            validator: (value) {
                              if (value == null || value.isEmpty) {
                                return 'Polje je obavezno';
                              }
                              if (value.length > 100) {
                                return 'Neispravan unos';
                              }
                              return null;
                            }),
                      ),
                      const SizedBox(width: 60),
                      Expanded(
                        child: Row(
                          children: [
                            Expanded(
                              flex: 9,
                              child: TextFormField(
                                controller: widget
                                    .controllers.accomodationTypePrices[index],
                                decoration: InputDecoration(
                                  labelText: index == 0 ? 'Cijena' : null,
                                ),
                                keyboardType: TextInputType.number,
                                validator: (value) {
                                  if (value == null || value.isEmpty) {
                                    return 'Polje je obavezno';
                                  }
                                  final n = num.tryParse(value);
                                  if (n == null) {
                                    return 'Unesite validan broj';
                                  }
                                  return null;
                                },
                              ),
                            ),
                            const Expanded(
                              flex: 1,
                              child: Text('KM',
                                  style: TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.w400)),
                            ),
                          ],
                        ),
                      ),
                      const SizedBox(width: 60),
                      Expanded(
                        child: Align(
                          alignment: Alignment.centerLeft,
                          child: IconButton(
                            icon: const Icon(Icons.close),
                            onPressed: () {
                              if (index > 0){
                              _removeArrangementPriceRow(index);
                              }
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
                      addArrangementPrices();
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

  void _removeArrangementPriceRow(int index) {
    setState(() {
      widget.controllers.accomodationTypes.removeAt(index);
      widget.controllers.accomodationTypePrices.removeAt(index);
      widget.controllers.accomodationTypeIds.removeAt(index);
    });
  }

  void addArrangementPrices() {
    setState(() {
      widget.controllers.accomodationTypePrices.add(TextEditingController());
      widget.controllers.accomodationTypes.add(TextEditingController());
      widget.controllers.accomodationTypeIds.add(null);
    });
  }
}
