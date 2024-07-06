import 'package:flutter/material.dart';

class DestinatiosFormControllers {
  List<TextEditingController> idControllers = [];
  List<TextEditingController> destinationControllers = [];
  List<TextEditingController> countryControllers = [];
  List<TextEditingController> arrivalDateTimeControllers = [];
  List<TextEditingController> departureDateTimeControllers = [];
  List<TextEditingController> hotelNameControllers = [];
  List<TextEditingController> cityControllers = [];
  List<TextEditingController> postalCodeControllers = [];
  List<TextEditingController> streetControllers = [];
  List<TextEditingController> houseNumberControllers = [];
  List<bool> isHotelIncluded = [];

  void addDestination() {
    idControllers.add(TextEditingController());
    destinationControllers.add(TextEditingController());
    countryControllers.add(TextEditingController());
    arrivalDateTimeControllers.add(TextEditingController());
    departureDateTimeControllers.add(TextEditingController());
    hotelNameControllers.add(TextEditingController());
    cityControllers.add(TextEditingController());
    postalCodeControllers.add(TextEditingController());
    streetControllers.add(TextEditingController());
    houseNumberControllers.add(TextEditingController());
    isHotelIncluded.add(false);
  }
}
