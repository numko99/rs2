import 'dart:convert';
import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/helpers/image_helper.dart';
import 'package:ui/models/destinatios_form_controllers.dart';

class DestinationFormPage extends StatefulWidget {
    final DestinatiosFormControllers controllers;

  const DestinationFormPage({
    super.key,
    required this.controllers
  });

  @override
  State<DestinationFormPage> createState() =>
      _DestinationFormPageState();
}

class _DestinationFormPageState extends State<DestinationFormPage> {
  @override
  void initState() {
    super.initState();

    if (widget.controllers.destinationControllers.isEmpty) {
      addDestination();
    }
  }

   void _removeDestinationRow(int index) {
    setState(() {
      widget.controllers.idControllers.removeAt(index);
      widget.controllers.destinationControllers.removeAt(index);
      widget.controllers.countryControllers.removeAt(index);
      widget.controllers.arrivalDateTimeControllers.removeAt(index);
      widget.controllers.departureDateTimeControllers.removeAt(index);
      widget.controllers.hotelNameControllers.removeAt(index);
      widget.controllers.cityControllers.removeAt(index);
      widget.controllers.postalCodeControllers.removeAt(index);
      widget.controllers.streetControllers.removeAt(index);
      widget.controllers.houseNumberControllers.removeAt(index);
      widget.controllers.isHotelIncluded.removeAt(index);
    });
  }

  void addDestination() {
    setState(() {
      widget.controllers.idControllers.add(TextEditingController());
      widget.controllers.destinationControllers.add(TextEditingController());
      widget.controllers.countryControllers.add(TextEditingController());
      widget.controllers.arrivalDateTimeControllers.add(
      TextEditingController());
      widget.controllers.departureDateTimeControllers.add(TextEditingController());
      widget.controllers.hotelNameControllers.add(TextEditingController());
      widget.controllers.cityControllers.add(TextEditingController());
      widget.controllers.postalCodeControllers.add(TextEditingController());
      widget.controllers.streetControllers.add(TextEditingController());
      widget.controllers.houseNumberControllers.add(TextEditingController());
      widget.controllers.isHotelIncluded.add(false);
    });
  }
  
  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Column(
            children: List.generate(
                widget.controllers.destinationControllers.length, (int index) {
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
                        if (widget.controllers.destinationControllers.length > 1) {
                          _removeDestinationRow(index);
                        }
                      },
                    )
                  ],
                ),
                Row(
                  children: [
                    Expanded(
                      child: TextFormField(
                        controller: widget.controllers.destinationControllers[index],
                        decoration: const InputDecoration(labelText: 'Destinacija'),
                          validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                           if (value.length > 100) {
                            return 'Neispravan unos';
                          }
                          return null;
                        },
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: TextFormField(
                      controller: widget.controllers.countryControllers[index],
                      decoration: const InputDecoration(labelText: 'Država'),
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Polje je obavezno';
                        }
                        if (value.length > 100) {
                          return 'Neispravan unos';
                        }
                        return null;
                      },
                    )
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      child: Row(
                        children: [
                          Expanded(
                            child: TextFormField(
                              controller: widget.controllers
                                  .arrivalDateTimeControllers[index],
                              decoration: InputDecoration(
                                labelText: 'Vrijeme dolaska',
                                suffixIcon: Icon(Icons.calendar_today),
                              ),
                              readOnly: true, // Sprečava pojavu tastature
                              onTap: () => DateTimeHelper.selectDateAndTime(
                                  context, widget.controllers
                                      .arrivalDateTimeControllers[index]),
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
                            child: TextFormField(
                              controller: widget.controllers
                                  .departureDateTimeControllers[index],
                              decoration: InputDecoration(
                                labelText: 'Vrijeme odlaska',
                                suffixIcon: Icon(Icons.calendar_today),
                              ),
                              readOnly: true, // Sprečava pojavu tastature
                              onTap: () => DateTimeHelper.selectDateAndTime(
                                  context, widget.controllers
                                      .departureDateTimeControllers[index]),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                return null;
                              },
                            ),
                          )
                        ],
                      ),
                    )
                  ],
                ),
                const SizedBox(height: 20),
                CheckboxListTile(
                  title: Text('Uključen hotel', style: TextStyle(fontSize: 15)),
                  value: widget.controllers.isHotelIncluded[index],
                  onChanged: (bool? value) {
                    setState(() {
                      widget.controllers.isHotelIncluded[index] = value!;
                    });
                  },
                  controlAffinity: ListTileControlAffinity
                      .leading, // Pozicionira checkbox na početak
                ),
                if (widget.controllers.isHotelIncluded[index] ?? false)
                  Column(
                    children: [
                      Row(
                        children: [
                          Expanded(
                            child: TextFormField(
                              controller: widget
                                  .controllers.hotelNameControllers[index],
                              decoration: const InputDecoration(
                                  labelText: 'Naziv hotela'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 30) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            child: TextFormField(
                              controller: widget.controllers.cityControllers[index],
                              decoration:
                                  const InputDecoration(labelText: 'Grad'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 30) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            child: TextFormField(
                              controller: widget
                                  .controllers. postalCodeControllers[index],
                              decoration: const InputDecoration(
                                  labelText: 'Poštanski broj'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 10) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 10),
                      Row(
                        children: [
                          Expanded(
                            child: TextFormField(
                              controller: widget.controllers.streetControllers[index],
                              decoration:
                                  const InputDecoration(labelText: 'Ulica'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 30) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            child: TextFormField(
                              controller: widget
                                  .controllers.houseNumberControllers[index],
                              decoration: const InputDecoration(
                                  labelText: 'Kućni broj'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 5) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
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
                addDestination();
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